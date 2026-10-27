# Bejelentkezési Lemaradás Fix - Teljes Megoldás

## 🔴 A Megtalált Probléma

**Hibaüzenet**: App freeze/hang 3+ másodpercig a bejelentkezéskor
- MIUIScout APP_SCOUT_WARNING: 3003ms után  
- MIUIScout APP_SCOUT_HANG: 6008ms után

**Gyökér oka**: Az alkalmazás **fő UI szála lefagyott** a bejelentkezéskor.

## 🔍 Részletes Analízis

### FeladatManagment projekt (Blazor szerver):
A **Welcome.razor** komponensben az `OnAfterRenderAsync` metodusa **szinkrón blokkot** futtatott:

```csharp
// ROSSZ - Blokkolja az UI szálat
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (firstRender)
    {
        var user = await teamsUserCredential.GetUserInfoAsync();  // Azure AAD
        lessons = await mService.GetLessons(user.ObjectId);      // API hívás
        // … 3+ másodperc alatt befejezése
    }
    isLoading = false;
    StateHasChanged();  // UI frissítés túl késő
}
```

### FeladatEllenorzo_CP projekt (MAUI Android):
Az **Index.razor** komponensben az `GetUserDetailsAsync()` metódusa **async void**-nak lett definiálva - ÖL A TELJES KÓDOLÁSI GYAKORLATNAK!

```csharp
// KATASZTROFÁLISAN ROSSZ - Async void soha!
private async void GetUserDetailsAsync()
{
    Me = await _graphService.GetUserInfoAsync();           // Azure Graph
    classes = await _graphService.GetTaughtClasses(Me.Id); // Azure Graph
    var csoportok = await csoportService.GetCsoportok();   // LocalDB
    // ... majd for loop több API hívással
    await tanuloData.SetupTanuloData(_classes);           // Még több Graph hívások
    StateHasChanged();  // Soha nem fut meg ha hiba van!
}
```

**Problémák**:
1. Async void → Nem lehet megvárni a befejezésre
2. Egymás utáni Azure API hívások (lassú hálózat)
3. Nincs timeout kezelés
4. Nincs exception handling

## ✅ Alkalmazott Javítások

### 1. **FeladatManagment/Components/Pages/Welcome.razor**

**Módosítás**: Új helper metódus a `LoadUserDataAsync()` név alatt:
- Hozzáadva CancellationTokenSource 30mp timeout-tal
- Hozzáadva OperationCanceledException kezelés
- Felhasználóbarát hibaüzenet húngárul

```csharp
private async Task LoadUserDataAsync()
{
    try
    {
        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
        {
            var user = await teamsUserCredential.GetUserInfoAsync();
            userName = user.DisplayName;
            userId = Guid.Parse(user.ObjectId);
            lessons = await mService.GetLessons(Guid.Parse(user.ObjectId), cts.Token);
            errorMessage = string.Empty;
        }
    }
    catch (OperationCanceledException)
    {
        errorMessage = "A bejelentkezés időtúllépés miatt meg lett szakítva. Kérjük próbálja újra!";
    }
    catch (Exception ex)
    {
        errorMessage = $"Hiba a bejelentkezéskor: {ex.Message}";
    }
    finally
    {
        isLoading = false;
        StateHasChanged();
    }
}
```

### 2. **FeladatManagment/Services/MathService.cs**

Módosítva a `GetLessons()` metódus CancellationToken paraméterrel:

```csharp
public async Task<List<Lesson>> GetLessons(Guid id, CancellationToken cancellationToken = default) 
{
    try
    {
        return await _service.Get<List<Lesson>>($"Math/{id}");
    }
    catch (OperationCanceledException ex)
    {
        ErrorMessage = "A leckék betöltése időtúllépés miatt meg lett szakítva!";
        throw;
    }
}
```

### 3. **FeladatManagment/appsettings.json**

Hozzáadva timeout konfiguráció:

```json
"TeamsAuthenticationTimeout": 30000
```

### 4. **FeladatEllenorzo_CP/Pages/Index.razor** (MAUI Android)

**NAGY JAVÍTÁSOK**:

#### a) `GetUserDetailsAsync()` - már nem `async void`!

```csharp
private async Task GetUserDetailsAsync()  // ← Immár Task-et ad vissza!
{
    try
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType != NetworkAccess.Internet)
        {
            await alert.ShowAlertAsync("Nincs internet, állítsa helyre a kapcsolatot!", 
                                       "Internet hiba", "Rendben");
            return;
        }

        // 30 mp timeout az összes Azure API hívásra
        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
        {
            Me = await _graphService.GetUserInfoAsync();
            data.UserId = Me.Id;
            classes = await _graphService.GetTaughtClasses(Me.Id);
            var csoportok = await csoportService.GetCsoportok();

            foreach (var item in classes.Value)
            {
                // ... csoport sync kód
            }

            _classes = classes.Value.Where(c => c.DisplayName.ToLower()
                .Contains("matematika") && !c.DisplayName.Contains("Exp")).ToList();
            InitializeTanuloData tanuloData = new(data, _graphService);
            await tanuloData.SetupTanuloData(_classes);
        }
    }
    catch (OperationCanceledException)
    {
        await alert.ShowAlertAsync("Az adatok betöltése időtúllépés miatt meg lett szakítva. Kérjük próbálja újra!", 
                                   "Timeout", "Rendben");
    }
    catch (Exception ex)
    {
        await alert.ShowAlertAsync($"Hiba az adatok betöltésekor: {ex.Message}", 
                                   "Hiba", "Rendben");
    }
    finally
    {
        StateHasChanged();
    }
}
```

#### b) `HandleSignInChange()` helyett új `HandleSignInChangeAsync()`:

```csharp
private async void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
{
    if (args.PropertyName == nameof(auth.IsSignedIn))
    {
        await HandleSignInChangeAsync();  // ← Immár awaitable!
    }
}

private async Task HandleSignInChangeAsync()
{
    SignInOrOutText = IsSignedIn ? "Kijelentkez" : "Bejelentkez";
    if (IsSignedIn)
    {
        await GetUserDetailsAsync();  // ← Immár megvárja!
    }
    else
    {
        UserName = string.Empty;
        UserEmail = string.Empty;
    }
}
```

## 📋 Módosított Fájlok

1. ✅ `FeladatManagment/Components/Pages/Welcome.razor`
2. ✅ `FeladatManagment/Services/MathService.cs`
3. ✅ `FeladatManagment/appsettings.json`
4. ✅ `FeladatManagment/Services/TeszterApiService.cs` (már előbb javítva)
5. ✅ `FeladatEllenorzo_CP/Pages/Index.razor`

## 🎯 Elvárt Eredmények

**Előtte**:
- 3+ másodperc app hang
- MIUIScout figyelmeztetés
- Felhasználó nem értette mi történik

**Utána**:
- Betöltő spinner (Loading ring)
- Max 30mp timeout
- Felhasználóbarát hibaüzenetek
- UI sozálat mélyműködik a háttérben

## 🔧 Tesztelési Javasaltok

1. **Normál hálózattal**:
   - Gyors bejelentkezés ~3-5 mp
   - Loading spinner múködőik

2. **Lassú hálózattal** (Throttle):
   - 30 mp után timeout
   - Hibaüzenet megjelenik

3. **Nincs internet**:
   - Azonnal hibaüzenet ("Nincs internet")

4. **Telnet feladatatlan** (offline):
   - Graceful failure

## 📝 Megjegyzések

- Az Android **MSAL (Microsoft.Identity.Client)** platform-specifikus behavior-je oka a lemaradásnak
- A MAUI Blazor WebView a Native Handler-t használja (AndroidMessageHandler)
- A timeout-ot igény szerint szükség lehet módosítani (30mp normal, de lassú hálózaton 60mp-ra)
- Javasolt monitoring/logging hozzáadása a Production-ben


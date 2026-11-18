using Microsoft.Graph.Models;

namespace FeladatEllenorzo_CP
{
    public static class FeladatHelper
    {
        /// <summary>Alapértelmezett pont, ha a Teams-feladat nincs pontozva.</summary>
        public const float AlapertelmezettPont = 2;

        /// <summary>
        /// A Teams-feladatnál beállított maximális pontszám, vagy null, ha a feladat nincs pontozva.
        /// A GraphService.GetFeladatok "grading" mezőt is lekéri, enélkül a Grading mindig null.
        /// </summary>
        public static float? MaxPont(EducationAssignment? feladat)
            => feladat?.Grading is EducationAssignmentPointsGradeType p && p.MaxPoints is float max && max > 0 ? max : null;

        /// <summary>A pontmező kezdőértéke: a feladat maximuma, ennek hiányában 2.</summary>
        public static float AlapPont(EducationAssignment? feladat)
            => MaxPont(feladat) ?? AlapertelmezettPont;
    }
}

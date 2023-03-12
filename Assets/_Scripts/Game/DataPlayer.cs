using UnityEngine;

namespace Assets._Scripts.Game
{
    public class DataPlayer
    {
        public Vector3 Position;

        public bool IsTutorialPartComplete;
        public bool IsTutorialComplete;

        public int Level;

        public void SetPositionAndRotation(Vector3 position) => Position = position;

        public void SetLevel(int level) => Level = level;

        public void FirstPartTutorialComplete() => IsTutorialPartComplete = true;

        public void TutorialComplete() => IsTutorialComplete = true;
    }
}
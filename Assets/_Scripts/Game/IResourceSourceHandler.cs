namespace Assets._Scripts.Game
{
    public interface IResourceSourceHandler
    {
        float GetHealth();
        float GetMaxHealth();
        float GetTimeDamage();

        bool IsRecovery();
        bool IsDelayRecovery();

        void EnableView();
        void RecoveryEnable();
        void GetDamage(float damage);   
    }
}
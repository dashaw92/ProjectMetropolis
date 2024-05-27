using System;
using BepInEx;
using MoreSlugcats;

namespace ProjectMetropolis {
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class ProjectMetropolis : BaseUnityPlugin {
        public const string PluginGuid = "dashaw92.projectmetropolis";
        public const string PluginName = "The Metropolis Project";
        public const string PluginVersion = "0.0.1";

        private readonly ProjMetropolisOptionsMenu _options;
        
        public ProjectMetropolis() {
            _options = new ProjMetropolisOptionsMenu(this);
        }

        public void OnEnable() {
            On.RainWorld.OnModsInit += RWModInit;
            On.ScavengersWorldAI.AddScavenger += ScavengerSpawnHook;
            On.ScavengerAI.WeaponScore += ScavengerAIOnWeaponScore;
        }

        private int ScavengerAIOnWeaponScore(On.ScavengerAI.orig_WeaponScore orig, ScavengerAI self, PhysicalObject obj, bool pickupdropinsteadofweaponselection) {
            return obj is SingularityBomb ? 6 : orig(self, obj, pickupdropinsteadofweaponselection);
        }

        private void RWModInit(On.RainWorld.orig_OnModsInit orig, RainWorld self) {
            orig(self);
            try {
                MachineConnector.SetRegisteredOI(PluginGuid, _options);
            }
            catch (Exception) {
                Logger.LogWarning($"Failed to register OI for {PluginGuid}");
            }
        }
        
        private void ScavengerSpawnHook(On.ScavengersWorldAI.orig_AddScavenger orig, ScavengersWorldAI self, ScavengerAbstractAI scav) {
            if (UnityEngine.Random.Range(0, 100) < _options.ReplacementChance.Value) {
                var bomb = new AbstractPhysicalObject(self.world, MoreSlugcatsEnums.AbstractObjectType.SingularityBomb,
                    null, scav.parent.pos, self.world.game.GetNewID());
                self.world.GetAbstractRoom(scav.parent.pos).AddEntity(bomb);
                _ = new AbstractPhysicalObject.CreatureGripStick(scav.parent, bomb, 0, true);
            }

            orig(self, scav);
        }
    }
}
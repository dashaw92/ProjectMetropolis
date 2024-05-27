using Menu.Remix.MixedUI;
using UnityEngine;

namespace ProjectMetropolis {
    public class ProjMetropolisOptionsMenu : OptionInterface {
        private readonly ProjectMetropolis _mod;
        public readonly Configurable<int> ReplacementChance;

        public ProjMetropolisOptionsMenu(ProjectMetropolis mod) {
            _mod = mod;
            
            ReplacementChance = config.Bind("replacementChance", 1);
        }

        public override void Initialize() {
            base.Initialize();
            
            var tab = new OpTab(this, "Options");
            Tabs = new[] { tab };
            var options = new UIelement[] {
                new OpLabel(10f, 550f, "Config", true),
                new OpSlider(ReplacementChance, new Vector2(10f, 590f), 100)
                {
                    description = "Configure the chances scavengers will spawn with a singularity bomb"
                }
            };
            
            tab.AddItems(options);
        }
    }
}
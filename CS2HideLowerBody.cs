using System.Drawing;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using System.Collections.Generic;

namespace CS2HideLowerBody
{

    [MinimumApiVersion(50)]
    public class CS2HideLowerBody : BasePlugin
    {
        public override string ModuleName => "HideLowerBody";
        public override string ModuleAuthor => "DRANIX & dollan";
        public override string ModuleDescription => "Allows players to hide their first person legs model. (lower body view model)";
        public override string ModuleVersion => "1.0.2";
        public override void Load(bool hotReload)
        {
            this.RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);

            if (!hotReload) return;
        }

        public override void Unload(bool hotReload)
        {
            // Clean up if needed
        }

        [GameEventHandler]
        private static HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
        {
            CCSPlayerController controller = @event.Userid;

            if (!controller.IsValid || controller.IsBot || controller.TeamNum <= (byte)CsTeam.Spectator) return HookResult.Continue;

            SetPawnAlphaRender(controller);
            UpdatePlayer(controller);

            return HookResult.Continue;
        }

        private static void SetPawnAlphaRender(CCSPlayerController controller) => controller.PlayerPawn.Value.Render = Color.FromArgb(254,
                controller.PlayerPawn.Value.Render.R, controller.PlayerPawn.Value.Render.G, controller.PlayerPawn.Value.Render.B);

        private static void UpdatePlayer(CCSPlayerController controller)
        {
            const string classNameHealthShot = "weapon_healthshot";

            controller.GiveNamedItem(classNameHealthShot);

            var healthShot = controller.PlayerPawn.Value.WeaponServices!.MyWeapons.FirstOrDefault(weapon => weapon is { IsValid: true, Value: { IsValid: true, DesignerName: classNameHealthShot } });

            if (healthShot == null || !healthShot.IsValid) return;

            healthShot.Value.Remove();
        }
    }
}
using KSP.Localization;
using UnityEngine;
using ToolbarControl_NS;



namespace DeepFreeze
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        protected void Start()
        {
            ToolbarControl.RegisterMod(RSTUtils.AppLauncherToolBar.MODID, RSTUtils.AppLauncherToolBar.MODNAME);
        }
    }
}

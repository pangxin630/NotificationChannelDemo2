using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationChannelDemo2.Droid.PlatformInfoImp))]
namespace NotificationChannelDemo2.Droid
{
    public class PlatformInfoImp : IPlatformInfo
    {
        public void SetBadgeOfSumsung(int count)
        {
            // 获取你当前的应用
            String launcherClassName = getLauncherClassName(Android.App.Application.Context);
            if (launcherClassName == null)
            {
                return;
            }
            Intent intent = new Intent("android.intent.action.BADGE_COUNT_UPDATE");
            intent.PutExtra("badge_count", count);
            intent.PutExtra("badge_count_package_name", Android.App.Application.Context.PackageName);
            intent.PutExtra("badge_count_class_name", launcherClassName);
            Android.App.Application.Context.SendBroadcast(intent);
        }

        public String getLauncherClassName(Context context)
        {

            PackageManager pm = context.PackageManager;

            Intent intent = new Intent(Intent.ActionMain);
            intent.AddCategory(Intent.CategoryLauncher);

            var resolveInfos = pm.QueryIntentActivities(intent, 0);
            foreach (ResolveInfo resolveInfo in resolveInfos)
            {
                String pkgName = resolveInfo.ActivityInfo.ApplicationInfo.PackageName;
                if (pkgName.Equals(context.PackageName))
                {
                    String className = resolveInfo.ActivityInfo.Name;
                    return className;
                }
            }
            return null;
        }


        public void setBadgeCount(int count, int iconResId)
        {
            // TODO 生成器模式重构
            if (count <= 0)
            {
                count = 0;
            }
            else
            {
                count = Math.Max(0, Math.Min(count, 99));
            }
            if (Build.Manufacturer.ToLower().Contains("xiaomi"))
            {
                //setBadgeOfMIUI(context, count, iconResId);
            }
            else if (Build.Manufacturer.ToLower().Contains("sony"))
            {
                //setBadgeOfSony(context, count);
            }
            else if (Build.Manufacturer.ToLower().Contains("samsung") ||
                  Build.Manufacturer.ToLower().Contains("lg"))
            {
                //setBadgeOfSumsung(context, count);
            }
            else if (Build.Manufacturer.ToLower().Contains("htc"))
            {
                //setBadgeOfHTC(context, count);
            }
            else if (Build.Manufacturer.ToLower().Contains("nova"))
            {
                //setBadgeOfNova(context, count);
            }
            else
            {
                Android.Widget.Toast.MakeText(Android.App.Application.Context, "Not Found Support Launcher", Android.Widget.ToastLength.Long).Show();
            }
        }
    }
}
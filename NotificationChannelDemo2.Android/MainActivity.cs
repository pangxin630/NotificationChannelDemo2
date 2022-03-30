using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;
using Xamarin.Forms;

namespace NotificationChannelDemo2.Droid
{
    [Activity(Label = "NotificationChannelDemo2", Icon = "@mipmap/icon", Theme = "@style/MainTheme", 
        MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            CreateNotificationFromIntent(Intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra(AndroidNotificationManager.TitleKey);
                string message = intent.GetStringExtra(AndroidNotificationManager.MessageKey);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
            StartServer(this, intent);

        }


        public static void StartServer(ContextWrapper context, Intent intent)
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {

                context.StartForegroundService(intent);

            }
            else
            {
                context.StartService(intent);
            }
        }


        public void Message()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                // 1. 创建一个通知(必须设置channelId)
                Context context = this.Application.ApplicationContext;
                String channelId = "ChannelIdTeset"; // 通知渠道
                Notification notification = new Notification.Builder(context)
                        .SetChannelId(channelId)
                        //.SetSmallIcon(R.mipmap.icon_bill_64x64)
                        .SetContentTitle("通知标题")
                        .SetContentText("通知内容")
                        .Build();
                // 2. 获取系统的通知管理器(必须设置channelId)
                NotificationManager notificationManager = (NotificationManager)context
                        .GetSystemService(NotificationService);
                NotificationChannel channel = new NotificationChannel(
                        channelId,
                        "通知的渠道名称",
                        NotificationImportance.Default);
                notificationManager.CreateNotificationChannel(channel);
                // 3. 发送通知(Notification与NotificationManager的channelId必须对应)
                //notificationManager.Notify(id, notification);
            }
        }

        private void madMode(int count)
        {
            Intent intent = new Intent(Intent.ActionMain, null);
            intent.AddCategory(Intent.CategoryLauncher);

            var list = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.Activities);
            for (int i = 0; i < list.Count; i++)
            {
                ActivityInfo activityInfo = list[i].ActivityInfo;
                String activityName = activityInfo.Name;
                String packageName = activityInfo.ApplicationInfo.PackageName;
                //BadgeUtil.setBadgeOfMadMode(getApplicationContext(), count, packageName, activityName);
            }
        }

        private void setBadgeOfSumsung(int count)
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

        public static String getLauncherClassName(Context context)
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


    }
}
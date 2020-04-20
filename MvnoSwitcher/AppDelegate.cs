﻿using Foundation;
using MvnoSwitcher.Http;
using System;
using System.IO;
using System.Net;
using System.Threading;
using UIKit;

namespace MvnoSwitcher
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        private bool _isInBackground;
        private nint _backgroundTaskId;


        public static AppDelegate Current => (AppDelegate)UIApplication.SharedApplication.Delegate;

        public override UIWindow Window { get; set; }
        public HttpServer HttpServer { get; set; }
        public AppConfig AppConfig { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            SetupHttpListener();
            AppConfig = new AppConfig();
            AppConfig.Load();
            return true;
        }

        private void SetupHttpListener()
        {
            HttpServer = new HttpServer();
            HttpServer.Start();
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
            _backgroundTaskId = application.BeginBackgroundTask("MvnoSwitcher local http server", () =>
            {
                _isInBackground = false;
            });
            _isInBackground = true;
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
            if (_isInBackground)
            {
                application.EndBackgroundTask(_backgroundTaskId);
                _isInBackground = false;
            }
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}
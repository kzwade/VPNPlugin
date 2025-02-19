﻿using System;
using System.Collections.Generic;
using System.Linq;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;
using System.Net.Http;

namespace VPNPlugin
{
    [ApiVersion(2, 1)]
    public class VPNPlugin : TerrariaPlugin
    {
        
        public VPNPlugin(Main game) : base(game)
        {

        }

        private static readonly HttpClient client = new HttpClient();

        public override void Initialize()
        {
            ServerApi.Hooks.ServerJoin.Register(this, OnJoinAsync);
        }

        async void OnJoinAsync(JoinEventArgs args)
        {
            var response = await client.PostAsync("http://check.getipintel.net/check.php?ip=" + TShock.Players[args.Who].IP + "&contact=wadeon120fps@gmail.com", null);

            var responseString = await response.Content.ReadAsStringAsync();

            int responseInt;

            int.TryParse(responseString, out responseInt);

            if (responseInt == 1)
            {
                TShock.Players[args.Who].Disconnect("AntiProxy: Proxy connections are not permitted.");
            }

            if (TShock.Players[args.Who].Name.Length > 20)
            {
                TShock.Players[args.Who].Disconnect("Invalid Name! Must be less than 20 characters.");
                return;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.ServerJoin.Deregister(this, OnJoinAsync);
            }
            base.Dispose(disposing);
        }
    }
}
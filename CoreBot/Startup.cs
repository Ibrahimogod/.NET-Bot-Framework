﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.12.2

using CoreBot.Bots;
using CoreBot.Dialogs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreBot
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
            // Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
            services.AddSingleton<IStorage, MemoryStorage>();
            // Create the User state. (Used in this bot's Dialog implementation.)
            var storage = new MemoryStorage();
            var userState = new UserState(storage);
            services.AddSingleton(userState);
            services.AddSingleton<UserState>();
            // Create the Conversation state. (Used by the Dialog system itself.)
            var conversationState = new ConversationState(storage);
            services.AddSingleton(conversationState);
            // Register LUIS recognizer
            services.AddSingleton<FlightBookingRecognizer>();
            // Register the BookingDialog.
            services.AddSingleton<BookingDialog>();
            services.AddSingleton<IStorage, MemoryStorage>();
            // Create the Conversation state.
            // The MainDialog that will be run by the bot.
            services.AddSingleton<MainDialog>();
            services.AddSingleton<MyBotDialog>();
            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            //services.AddTransient<IBot, DialogAndWelcomeBot<MainDialog>>();
            services.AddTransient<IBot, DialogAndWelcomeBot<MyBotDialog>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
    }
}

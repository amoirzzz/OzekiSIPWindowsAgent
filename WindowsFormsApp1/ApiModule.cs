﻿// ExampleApiModule.cs
using Nancy;
using System;
using System.Windows.Forms;
using SIPWindowsAgent;

public class ExampleApiModule : NancyModule
{
    private bool isFormShown = false;

    public ExampleApiModule()
    {
        Get("/api/callMain/{Id}", parameters => HandleCall(parameters.Id));
        Get("/api/isformshown", _ => IsFormShown());

    }

    private dynamic HandleCall(string Id)
    {
        MainForm frm = MainForm.SingleFormInstance;
        frm.CreatCallFromApi(Id);
        return HttpStatusCode.OK;
    }
    private dynamic IsFormShown()
    {
        isFormShown=true;
        SynchronizationManager.Instance.SetFormShown();
        return isFormShown;
    }
}

public class CustomBootstrapper : DefaultNancyBootstrapper
{
    protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
    {
        base.ApplicationStartup(container, pipelines);

        pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
        {
            ctx.Response.WithHeader("Access-Control-Allow-Origin", "https://my.barsasoft.com")
                        .WithHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
                        .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type")
                        .WithHeader("Access-Control-Allow-Credentials", "true");
            
        });
        pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
        {
            if (ctx.Request.Method == "OPTIONS")
            {
                ctx.Response = new Nancy.Response();
            }
        });
    }
}



//public class CustomBootstrapper : DefaultNancyBootstrapper
//{
//    protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
//    {
//        base.ApplicationStartup(container, pipelines);

//        // Enable CORS for all routes
//        pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
//        {
//            ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
//                        .WithHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
//                        .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
//        });
//        pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
//        {
//            if (ctx.Request.Method == "OPTIONS")
//            {
//                ctx.Response = new Nancy.Response();
//            }
//        });
//    }
//}
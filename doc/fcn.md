# FCN

[From](https://learn.microsoft.com/en-us/dotnet/api/system.web.configuration.fcnmode?view=netframework-4.8.1):

> Specifies behavior for **file change notification (FCN)** in the application.

## Configuration

```xml
<configuration>
  <system.web>
    <httpRuntime targetFramework="4.8" fcnMode="Disabled" />
  </system.web>
</configuration>
```

`fcnMode="Disabled"`:
- No file changes are monitored _except_ root `web.config`
  - If you change `web.config` in a subfolder, the app pool will not recycle
  - If you change any of the `.cshtml` files, the app pool will not recycle
  - No file changes are monitored in the `bin` folder

If you use `fcnMode="Disabled"`, then you **must make sure that application is restarted after deployment**.

_Note_: **For development**, you most likely want to set `fcnMode="Default"` / `fcnMode="Single"` to allow it to automatically reflect the file changes.

## Deployment logs

Example deployment log 1:

```text
2>Start Web Deploy Publish the Application/package to https://webappfilesandfolders0000010.scm.azurewebsites.net/msdeploy.axd?site=webappfilesandfolders0000010 ...
2>Adding ACLs for path (webappfilesandfolders0000010)
2>Adding ACLs for path (webappfilesandfolders0000010)
2>Updating file (webappfilesandfolders0000010\Views\Home\Uptime.cshtml).
2>Adding ACLs for path (webappfilesandfolders0000010)
2>Adding ACLs for path (webappfilesandfolders0000010)
2>Publish Succeeded.
2>Web App was published successfully https://webappfilesandfolders0000010.azurewebsites.net/
========== Build: 1 succeeded, 0 failed, 0 up-to-date, 0 skipped ==========
========== Elapsed 00:09,442 ==========
========== Publish: 1 succeeded, 0 failed, 0 skipped ==========
========== Elapsed 00:09,443 ==========
```

Example deployment updated `\Views\Home\Uptime.cshtml` file but it won't be reflected for the end users before application restart.

Example deployment log 2:

```text
2>Start Web Deploy Publish the Application/package to https://webappfilesandfolders0000010.scm.azurewebsites.net/msdeploy.axd?site=webappfilesandfolders0000010 ...
2>Adding ACLs for path (webappfilesandfolders0000010)
2>Adding ACLs for path (webappfilesandfolders0000010)
2>Updating file (webappfilesandfolders0000010\Areas\HelpPage\Views\Web.config).
2>Updating file (webappfilesandfolders0000010\bin\WebAppDotnetFramework.dll).
2>Updating file (webappfilesandfolders0000010\bin\WebAppDotnetFramework.pdb).
2>Updating file (webappfilesandfolders0000010\Views\Home\Uptime.cshtml).
2>Updating file (webappfilesandfolders0000010\Views\Web.config).
2>Updating file (webappfilesandfolders0000010\Web.config).
2>Adding ACLs for path (webappfilesandfolders0000010)
2>Adding ACLs for path (webappfilesandfolders0000010)
2>Publish Succeeded.
2>Web App was published successfully https://webappfilesandfolders0000010.azurewebsites.net/
```

This deployment updated `web.config` file and the app pool was recycled and also `\Views\Home\Uptime.cshtml` changes were reflected for the end users.

Example deployment log 3:

```text
2>Start Web Deploy Publish the Application/package to https://webappfilesandfolders0000010.scm.azurewebsites.net/msdeploy.axd?site=webappfilesandfolders0000010 ...
2>Adding ACLs for path (webappfilesandfolders0000010)
2>Adding ACLs for path (webappfilesandfolders0000010)
2>Updating file (webappfilesandfolders0000010\Content\Site.css).
2>Adding ACLs for path (webappfilesandfolders0000010)
2>Adding ACLs for path (webappfilesandfolders0000010)
2>Publish Succeeded.
2>Web App was published successfully https://webappfilesandfolders0000010.azurewebsites.net/
```

This deployment updated only `\Content\Site.css` and it's reflected to the end users without application restart (it's static file and not compiled resource).

## Links

https://techcommunity.microsoft.com/t5/iis-support-blog/troubleshooting-performance-problems-related-to-application/ba-p/1131600

https://www.rahulpnath.com/blog/azure-web-restarting-automatically-due-to-overwhelming-change-notification/

https://www.dnnsoftware.com/community-blog/cid/154980/aspnet-file-change-notifications-and-dnn

https://dnnsupport.dnnsoftware.com/hc/en-us/articles/4407122102162-How-to-disable-File-Change-Notifications-FCNMode-To-Improve-Performance

https://shazwazza.com/post/all-about-aspnet-file-change-notification-fcn/

https://learn.microsoft.com/en-us/archive/blogs/tmarq/asp-net-file-change-notifications-exactly-which-files-and-directories-are-monitored

https://learn.microsoft.com/en-us/dotnet/api/system.web.configuration.fcnmode

https://support.microsoft.com/en-us/topic/fix-asp-net-2-0-connected-applications-on-a-web-site-may-appear-to-stop-responding-9792cf77-62b8-61c7-aaca-26f8b696d099

https://imageresizing.net/docs/fcnmode

https://wiki.mdriven.net/index.php/IIS_application_restart_problem

https://sitecorecommerce.wordpress.com/2020/10/26/azure-tips-for-sitecore-on-paas-disable-fcn/

https://iis-blogs.azurewebsites.net/hosterposter/Hosting-IIS-with-UNC-content-_2D00_-Network-BIOS-commands-and-other-errors

> *ASP.NET and File Change Notifications*
>
> Some other things to consider. One is that ASP.NET uses more notifications than html/static or classic ASP files.
> ASP.NET has to monitor a lot of files for an application, like Web.config, Global.asax, etc. 
> Given a file to monitor, it creates a directory monitor for the folder containing the file. 
> It will not monitor recursively (subdirectories). So if you have files in many different directories, 
> you get a bunch of directory monitors. Each directory monitor is an SMB work item.
>
> So the ASP.NET team released a QFE to help out. 
> The KB discusses different behaviors that are possible, from turning off file change notifications
> (meaning *you have to build some other way of monitoring for changes or you need to unload the app on changes*)
> to using a single monitor for the root of an application and all subdirectories. 
> Check out http://support.microsoft.com/?id=911272 for more details.
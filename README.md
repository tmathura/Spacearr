| Master | Dev | Current |
|-------|-------|-------|
| [![Build status](https://ci.appveyor.com/api/projects/status/v6fykek56q66lwqm/branch/master?svg=true)](https://ci.appveyor.com/project/tmathura/spacearr/branch/master) | [![Build status](https://ci.appveyor.com/api/projects/status/v6fykek56q66lwqm/branch/dev?svg=true)](https://ci.appveyor.com/project/tmathura/spacearr/branch/dev) | [![Build status](https://ci.appveyor.com/api/projects/status/v6fykek56q66lwqm?svg=true)](https://ci.appveyor.com/project/tmathura/spacearr) |

# Spacearr
Spacearr allows you to monitor your hard disk space on you computer via an Android phone or via a Windows UWP app. This is done without opening any ports on a computer but by using a message service from [Pusher](https://pusher.com/). The messaging service that [Pusher](https://pusher.com/) offers us is a "Pub/Sub" service, which is an asynchronous messaging service which is used by the computer and Android/UWP app to communicate to one another.

# Spacearr Android App
This is the app that is used on the Android phone to check the different drives on the computer. You can see the total space and as well as the free space. You will get notifications every 15 minutes if there is low space.

# Spacearr UWP App
This is the app that is used on the Windows computer to check the different drives on the computer. You can see the total space and as well as the free space.

# Spacearr Windows Service
This is the Windows service that you will install onto the computer so that the computer and Android/UWP app can communicate.

# Spacearr Windows Worker Service
This is the Windows worker service that you will install onto the computer so that the computer and Android/UWP app can communicate. It is basically the same as a Windows service, but more modern and uses the [.Net Core](https://en.wikipedia.org/wiki/.NET_Core) Framework. See this Stackoverflow [answer](https://stackoverflow.com/questions/59636097/c-sharp-worker-service-vs-windows-service#:~:text=Both%20are%20real%20services.,and%20stops%20with%20the%20application.) for more information.
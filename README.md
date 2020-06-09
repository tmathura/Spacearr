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

## Getting Started

[![Installation](https://img.shields.io/badge/wiki-installation-brightgreen.svg?maxAge=60&style=flat-square)](https://github.com/tmathura/Spacearr/wiki/Installation)
[![Setup Guide](https://img.shields.io/badge/wiki-setup_guide-orange.svg?maxAge=60&style=flat-square)](https://github.com/tmathura/Spacearr/wiki/Setup-Guide)
[![FAQ](https://img.shields.io/badge/wiki-FAQ-BF55EC.svg?maxAge=60&style=flat-square)](https://github.com/tmathura/Spacearr/wiki/FAQ)

* [Install Spacearr](https://github.com/tmathura/Spacearr/wiki/Installation)
* See the [Setup Guide](https://github.com/tmathura/Spacearr/wiki/Setup-Guide) for further configuration

## Downloads

| Release Type    | Branch: master (stable)                                                                                                                                                     | Branch: nightly (semi-unstable)                                                                                                                                                                |
|-----------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Binary Releases | [![GitHub Releases](https://img.shields.io/badge/downloads-releases-brightgreen.svg?maxAge=60&style=flat-square)](https://github.com/tmathura/Spacearr/releases)                 | [![AppVeyor Builds](https://img.shields.io/badge/downloads-nightly-green.svg?maxAge=60&style=flat-square)](https://ci.appveyor.com/project/tmathura/Spacearr/branch/dev/artifacts)    |                                                                                                                                                                                |

## Status

[![GitHub issues](https://img.shields.io/github/issues/tmathura/Spacearr.svg?maxAge=60&style=flat-square)](https://github.com/tmathura/Spacearr/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/tmathura/Spacearr.svg?maxAge=60&style=flat-square)](https://github.com/tmathura/Spacearr/pulls)
[![GNU GPL v3](https://img.shields.io/badge/license-GNU%20GPL%20v3-blue.svg?maxAge=60&style=flat-square)](http://www.gnu.org/licenses/gpl.html)
[![Copyright 2010-2017](https://img.shields.io/badge/copyright-2020-blue.svg?maxAge=60&style=flat-square)](https://github.com/tmathura/Spacearr)
[![Github Releases](https://img.shields.io/github/downloads/tmathura/Spacearr/total.svg?maxAge=60&style=flat-square)](https://github.com/tmathura/Spacearr/releases/)
[![Changelog](https://img.shields.io/github/commit-activity/w/tmathura/Spacearr.svg?style=flat-square)](/CHANGELOG.md#unreleased)

| Service  | Master                      | Develop                      |
|----------|:---------------------------:|:----------------------------:|
| AppVeyor | [![Build status](https://ci.appveyor.com/api/projects/status/v6fykek56q66lwqm/branch/master?svg=true)](https://ci.appveyor.com/project/tmathura/Spacearr/branch/master) | [![Build status](https://ci.appveyor.com/api/projects/status/v6fykek56q66lwqm/branch/dev?svg=true)](https://ci.appveyor.com/project/tmathura/Spacearr/branch/dev) |
|

## Features

### Current Features

* Supports platforms: Windows Services for backend, UWP & Android for front end.
* Allows you to view your hard disk space from your Android device.
* Get notifications on your Android device when your hard disk space is running low.
* No need to open ports on your desktop, just sign up to [Pusher](https://pusher.com/) and fill in those details when installing the service.

## Configuring the Development Environment

### Requirements

* [Visual Studio Community 2019](https://www.visualstudio.com/vs/community/)
* [Wix Toolset](https://wixtoolset.org/releases/)
* [Git](https://git-scm.com/downloads)

### Setup

* Make sure all the required software mentioned above are installed
* Clone the repository into your development machine ([*info*](https://help.github.com/desktop/guides/contributing/working-with-your-remote-repository-on-github-or-github-enterprise))

### Development

* Open `Spacearr.sln` in Visual Studio 2019
* Make sure `Spacearr.UWP` is set as the startup project
* Press `F5`
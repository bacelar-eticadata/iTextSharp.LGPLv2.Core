# iTextSharp.LGPLv2.Core.StrongName

[![Build status](https://ci.appveyor.com/api/projects/status/51cyuj418row2mb5/branch/master?svg=true)](https://ci.appveyor.com/project/stesee/itextsharp-lgplv2-core/branch/master) [![Codacy Badge](https://api.codacy.com/project/badge/Grade/17cafdf1d9354b95821b970a303120ce)](https://www.codacy.com/gh/Codeuctivity/iTextSharp.LGPLv2.Core?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Codeuctivity/iTextSharp.LGPLv2.Core&amp;utm_campaign=Badge_Grade)[![Nuget](https://img.shields.io/nuget/v/iTextSharp.LGPLv2.Core.StrongName)](https://www.nuget.org/packages/iTextSharp.LGPLv2.Core.StrongName/)

`iTextSharp.LGPLv2.Core` is an **unofficial** port of the last [LGPL version](http://www.gnu.org/licenses/old-licenses/lgpl-2.0-standalone.html) of the [iTextSharp (V4.1.6)](https://github.com/itextsharper/iTextSharp-4.1.6) to .NET Core.

## Usage

[Functional Tests](/src/iTextSharp.LGPLv2.Core.FunctionalTests)

## FAQ

 > The pdf is created, but when I try to view it, it says that the document is in use by another process.

 You should dispose the FileStream/MemoryStream [explicitly](https://github.com/Codeuctivity/iTextSharp.LGPLv2.Core/blob/master/src/iTextSharp.LGPLv2.Core.FunctionalTests/iTextExamples/Chapter11Tests.cs#L69). It won't be closed and disposed automatically at the end.

 > I can't find what would be the equivalent of the PdfTextExtractor class.

 PdfTextExtractor exists in v5.0.2+ with AGPL license (Current project is based on the iTextSharp 4.x, not 5.x).

 > It can't display Unicode characters.

 You can find more samples about how to define and use Unicode fonts [here](https://github.com/Codeuctivity/iTextSharp.LGPLv2.Core/blob/master/src/iTextSharp.LGPLv2.Core.FunctionalTests/iTextExamples/Chapter11Tests.cs).

 > Table rowspans don't work correctly.

 This version which is based on iTextSharp V4.1.6 doesn't support rowspans correctly (iTextSharp supports row spans correctly since v5.4.3, not before that). A solution based on the current version: use `nested tables` to simulate it.

 > iTextSharp.text.html.simpleparser.HTMLWorker does not exist.

 It has been renamed to [HtmlWorker](https://github.com/Codeuctivity/iTextSharp.LGPLv2.Core/blob/master/src/iTextSharp.LGPLv2.Core.FunctionalTests/HtmlWorkerTests.cs#L42).

## Note

To run this project on non-Windows-based operating systems, you will need to install `libgdiplus` too:

- Ubuntu 16.04 and above:
  - apt-get install libgdiplus
  - cd /usr/lib
  - ln -s libgdiplus.so gdiplus.dll
- Fedora 23 and above:
  - dnf install libgdiplus
  - cd /usr/lib64/
  - ln -s libgdiplus.so.0 gdiplus.dll
- CentOS 7 and above:
  - yum install autoconf automake libtool
  - yum install freetype-devel fontconfig libXft-devel
  - yum install libjpeg-turbo-devel libpng-devel giflib-devel libtiff-devel libexif-devel
  - yum install glib2-devel cairo-devel
  - git clone <https://github.com/mono/libgdiplus>
  - cd libgdiplus
  - ./autogen.sh
  - make
  - make install
  - cd /usr/lib64/
  - ln -s /usr/local/lib/libgdiplus.so libgdiplus.so
- Docker
  - RUN apt-get update \\

      && apt-get install -y libgdiplus
- MacOS
  - brew install mono-libgdiplus

      After installing the [Mono MDK](http://www.mono-project.com/download/#download-mac), Copy Mono MDK Files:
    - /Library/Frameworks/Mono.framework/Versions/4.6.2/lib/libgdiplus.0.dylib
    - /Library/Frameworks/Mono.framework/Versions/4.6.2/lib/libgdiplus.0.dylib.dSYM
    - /Library/Frameworks/Mono.framework/Versions/4.6.2/lib/libgdiplus.dylib
    - /Library/Frameworks/Mono.framework/Versions/4.6.2/lib/libgdiplus.la

      And paste them to: /usr/local/lib

## Licensing

You have three license choices when it comes to iTextSharp: LGPL/MPL, AGPL, or a commercial license. The LGPL/MPL license is only an option with the older 4.1.6 version (used here). After that version, they switched to a dual AGPL/Commercial.

If you need a more recent version, you either have to make your project open-source or pay the license fee.

What is it?
===========
DiskSpaceImage is a simple windows service that periodically gets the
available disk space on all fixed hard drive volumes, and writes the
result as a PNG image.

Where would I use it?
=====================
Windows Vista Media Center has an area on the Start page reserved for an
OEM logo.  Typically this wold be where the system builder (Dell, HP
etc.) would put their branding.

The path of the image is stored in the following registry key:

    HKLM\Software\Microsoft\Windows\CurrentVersion\Media Center\Start
Menu\OEMLogoURI

So the basic premise is that the service regenerates the image
periodically with the current available space, and writes it to the
location where the above registry key is pointing; which provides
at-a-glance monitoring of available space from within Media Center's 10'
UI.

Installing
==========
1. Drop the DiskSpaceImage.exe file somewhere on your local drive (eg.
C:\Program Files\DiskSpaceImage\DiskSpaceImage.exe.

2. Edit the setup.bat file and check that the path to your installation of
the .Net Framework 2.0 and to the DiskSpaceImage.exe is correct.

3. Run setup.bat, which simply installs the service.

4. Edit the DiskSpaceImage.exe.config file, and change the image size,
   font name/size/color, image path & frequency to your preference.

5. Any changes to the config settings require the service to be
   restarted (either on the command-line: net stop/start DiskSpaceImage;
or through the Services console (Right-click Computer -> Manage ->
Services and Applications -> Services -> Media Center Start Page Disk
Space Monitor).

6. It is recommended that the Service be set to autostart, but that's up
   to you.

7. Check that the service is correctly running and updating the PNG file
   at the specified interval.

8. Edit the registry key above, and point it at your PNG file (eg.
   file://C:\Program Files\DiskSpaceImage\DiskSpace.png)

9. Launch Media Center.  If all has gone well, you should see your image
   in the bottom right-hand corner of the start page.

Uninstalling
============
1. Clear the OEMLogoURI registry key.

2. Stop the service.

3. Edit the setup.bat file and add the "/u" parameter to the installutil
   command (eg. installutil /u C:\Program
Files\DiskSpaceImage\DiskSpaceImage.exe)

4. Run setup.bat

5. Delete the installation folder

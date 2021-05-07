# CacheImagesNET5



1- groupName: it is the name that you will give the folder where the images related to the group will be stored (for example: employees, logos, covers, etc.)

2- inside the folder (groupName) will be all images stored in the id_url.format format

3- For now it is only possible to delete the images before downloading or before using them in the application. So think carefully about your methods.

4- The search for images is done through a Uri (url) informed by the user, groupName and the specific Id of the group. If you request a search and the image is not found in the folder, you will be prompted to automatically download that image again using the search method.

5- The images are stored forever in your folder until they are deleted by the method or manually deleting in the application folder.

6- The folders and files created by CacheImage are set to read-only and are hidden to increase the security level of the application.

# CacheImagesNET5



1- groupName: it is the name that you will give the folder where the images related to the group will be stored (for example: employees, logos, covers, etc.)

2- inside the folder (groupName) will be all images stored in the id_url.format format

3- For now it is only possible to delete the images before downloading or before using them in the application. So think carefully about your methods.

4- The search for images is done through a Uri (url) informed by the user, groupName and the specific Id of the group. If you request a search and the image is not found in the folder, you will be prompted to automatically download that image again using the search method.

5- The images are stored forever in your folder until they are deleted by the method or manually deleting in the application folder.

6- The folders and files created by CacheImage are set to read-only and are hidden to increase the security level of the application.


# EXAMPLES



= > DOWNLOAD LIST IMAGE ( RETURN BOOL )

            // create a new tuple instance in the format => uri, string, int
            List<Tuple<Uri, string, int>> tupleList = new();

            // you can use a foreach here to add your items to the list
            var tuple = Tuple.Create(new Uri(@"https://i.imgur.com/WpOgwlx.jpg"), "Restaurantes", 1); // URL, GROUP_NAME, ID OF RESTAURANT
            var tuple2 = Tuple.Create(new Uri(@"https://i.imgur.com/VIw62oS.jpg"), "Restaurantes", 2); // URL, GROUP_NAME, ID OF RESTAURANT
            
            tupleList.Add(tuple);
            tupleList.Add(tuple2);
            // -------------------------------------------------------------------------
           
            //now request to download the entire list
            if (await CacheImage.DownloadImageList(tupleList))
                MessageBox.Show("Download Complete!");
                
= > DOWNLOAD ONE IMAGE ( RETURN BOOL ) 

            //create the tuple
            var tuple = Tuple.Create(new Uri(@"https://i.imgur.com/WpOgwlx.jpg"), "Restaurantes", 1);
            
            //now request to download this one image
            if (await CacheImage.DownloadOneImage(tuple))
                MessageBox.Show("Download Complete!");

=> SEARCH IMAGE ( THIS METHOD RETURNS THE IMAGE)
            
            int Id = 1;
            string groupName = "Restaurantes";
            Uri url = new Uri(@"https://i.imgur.com/WpOgwlx.jpg");

            pictureBox.Image = await CacheImage.SearchImage(url, "groupName", Id);
            
            
=> DELETE GROUP BY NAME


            //delete one group
            if (CacheImage.DeleteGroup("Restaurantes")) // This method return bool;
                MessageBox.Show("Deleted!");
                
               -----------------------/ /---------------------------------

            // or delete list of groups : 
                        List<string> groups = new();
                        groups.Add("Restaurantes"); // Example
                        groups.Add("Funcionarios"); // Example
                        if (CacheImage.DeleteGroups(groups)) // This method return bool;
                            MessageBox.Show("Groups deleted!");
           



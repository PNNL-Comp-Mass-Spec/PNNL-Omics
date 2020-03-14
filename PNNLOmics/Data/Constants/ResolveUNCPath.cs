using System.Collections.Generic;
using System.IO;
using System.Linq;
using PNNLOmics.Utilities;

namespace PNNLOmics.Data.Constants
{
    public class ResolveUNCPath
    {
        public class MappedDriveResolver
        {

            /// <summary>
            /// returns the path when it has a drive letter or a network path when it has a \\ in front
            /// </summary>
            /// <param name="pPath"></param>
            /// <returns></returns>
            public string ResolveToUNC(string pPath)
            {
                //Simple check for network path
                if (!pPath.StartsWith(@"\"))
                {
                    return pPath;
                }
                return ResolveToRootUNC(pPath);
            }

            /// <summary>
           /// updates the path when it starts with a // like a network drive
           /// </summary>
           /// <param name="pPath">path without a drive letter</param>
           /// <returns>full path</returns>
            public string ResolveToRootUNC(string pPath)
            {
                //1.  figure out the drive path using root and active directory

                //this contains the root + the first folder
                var rootOfMysteryPath = Directory.GetDirectoryRoot(pPath);

                //this contains all the path - the root
                var activeDirectoryPath = PathUtilities.AssemblyDirectory;

                //2.  replace // with space for splitting
                var charSeparator = Path.DirectorySeparatorChar;
                var activeDirectoryWords = activeDirectoryPath.Split(charSeparator).ToList();
                var rootOfMysteryWords = rootOfMysteryPath.Split(charSeparator).ToList();

                //3.  find intersecting folder between the root and the active directory
                var startOfRealPath = "";

                startOfRealPath = rootOfMysteryWords[rootOfMysteryWords.Count - 1];//this is the intersecting folder

                rootOfMysteryWords.RemoveAt(rootOfMysteryWords.Count-1);//remove extra folder

                var startOfRealPathIndex = activeDirectoryWords.IndexOf(startOfRealPath);//this is the index of the intersecting folder in the active directory

                //4.  re create path by first adding the network root
                var newPathWords = new List<string>();
                for (var i = 1; i < rootOfMysteryWords.Count; i++)
                {
                    newPathWords.Add(rootOfMysteryWords[i]);
                }

                //5.  next adding the rest of the active directory
                for (var i = startOfRealPathIndex; i < activeDirectoryWords.Count; i++)
                {
                    newPathWords.Add(activeDirectoryWords[i]);
                }

                //6.  replace "" with Path.DirectorySeparatorChar since that is what we split on at the start
                var newUNCPath = "";
                foreach (var word in newPathWords)
                {
                    if(word=="")
                    {
                        newUNCPath += Path.DirectorySeparatorChar;
                    }
                    else
                    {
                        newUNCPath += Path.DirectorySeparatorChar + word;
                    }
                }

                return newUNCPath;
            }
        }
    }
}

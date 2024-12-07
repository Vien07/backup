
namespace Steam.Core.FileManager.ExendtionMethod
{
    public static partial class ExendtionMethod
    {
        public static long GetDirectorySize(this System.IO.DirectoryInfo directoryInfo, bool recursive = true)
        {
            var startDirectorySize = default(long);
            if (directoryInfo == null || !directoryInfo.Exists)
                return startDirectorySize; //Return 0 while Directory does not exist.

            //Add size of files in the Current Directory to main size.
            foreach (var fileInfo in directoryInfo.GetFiles())
                System.Threading.Interlocked.Add(ref startDirectorySize, fileInfo.Length);

            if (recursive) //Loop on Sub Direcotries in the Current Directory and Calculate it's files size.
                System.Threading.Tasks.Parallel.ForEach(directoryInfo.GetDirectories(), (subDirectory) =>
            System.Threading.Interlocked.Add(ref startDirectorySize, GetDirectorySize(subDirectory, recursive)));

            return startDirectorySize;  //Return full Size of this Directory.
        }
    }
}

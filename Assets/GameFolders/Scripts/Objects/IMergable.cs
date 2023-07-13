namespace GameFolders.Scripts.Objects
{
    public interface IMergeable
    {
        public void Merge(IMergeable other);
        
        public bool CanMerge(IMergeable other);
        
        public bool IsMerged { get; }
        
        public void Unmerge();
        
        public void OnMerge(IMergeable other);
    }
}
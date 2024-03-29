﻿

namespace ProjectCollaborationPlatform.DAL.Data.Models
{
    public class Board : BaseEntity
    {
        public string Name {  get; set; } 
        public Guid ProjectID { get; set; }
        public Project Project { get; set; }
        public List<FunctionalityBlock> FunctionalityBlocksID { get; set; }
    }
}

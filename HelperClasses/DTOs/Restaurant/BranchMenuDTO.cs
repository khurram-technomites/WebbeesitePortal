using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class BranchMenuDTO
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ItemForMenuDTO> Items { get; set; } = new List<ItemForMenuDTO>();
    }

    public class ItemForMenuDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public List<ItemQuestions> MenuItemOptions { get; set; }
    }

    public class ItemQuestions
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public bool IsMain { get; set; }
        public int MaxLimit { get; set; }
        public bool IsRadioButton { get; set; }
        public string Type
        {
            get
            {
                return IsRadioButton ? "radio" : "checkbox";
            }
            set { }
        }
        public List<ItemQuestionOptions> MenuItemOptionValues { get; set; }
    }

    public class ItemQuestionOptions
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        //public bool IsPriceMain { get; set; }
    }
}


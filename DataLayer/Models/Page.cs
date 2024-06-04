using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DataLayer
{
    public class Page
    {
        [Key]
        public int PageID { get; set; }
        [Display(Name = "گروه خبری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int GroupID { get; set; }
        [Display(Name = "ناشر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50)]
        public string Publisher { get; set; }
        [Display(Name = "ناشر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50)]
        public string PublisherID { get; set; }
        [Display(Name = "عنوان خبر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100)]
        public string PageTitle { get; set; }
        [Display(Name = "توضیح مختصر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }
        [Display(Name = "متن خبر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public string ImageName { get; set; }
        [Display(Name = "تصویر")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "اسلایدر")]
        public bool ShowInSlider { get; set; }

        public int View { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "کلمات کلیدی")]
        public string Tags { get; set; }

        public virtual Group Group { get; set; }
        public virtual List<PageComment> PageComments { get; set; }

        public Page()
        {

        }
    }
}

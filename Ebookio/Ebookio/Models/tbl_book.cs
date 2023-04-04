
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Ebookio.Models
{

using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;


    public partial class tbl_book
{

    public int book_id { get; set; }
        [Required(ErrorMessage = "Please Enter Book Name")]
        [DisplayName("Book Name")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Enter only alphabets Allows..!")]
        public string book_title { get; set; }
        [RegularExpression("^[0-9]{13}$", ErrorMessage = "Enter Only 13 Digit ")]
        [DisplayName("ISBN NO")]
        public string isbn { get; set; }
        [DisplayName("No Of Pages")]
        [Required(ErrorMessage = "Please Enter No of Pages")]

        public Nullable<int> no_of_pages { get; set; }
        [DisplayName("Publish Date")]
        [Required(ErrorMessage = "Please Enter Select Publish Date")]
        public Nullable<System.DateTime> publish_date { get; set; }
        [DisplayName("Real Price")]
        [Required(ErrorMessage = "Please Enter Real Price")]
        public string real_price { get; set; }
        [DisplayName("Sell Price")]
        [Required(ErrorMessage = "Please Enter Sell Price")]
        public string sell_price { get; set; }
        [DisplayName("Cover Image")]
        public string cover_image { get; set; }
        [DisplayName("Publisher")]
        [Required(ErrorMessage = "Please Enter Publisher Name")]
        public Nullable<int> publisher_id { get; set; }
        [DisplayName("Author")]
        [Required(ErrorMessage = "Please Enter Author Name")]
        public Nullable<int> author_id { get; set; }
        [DisplayName("Language")]
        [Required(ErrorMessage = "Please Enter Language Name")]
        public Nullable<int> language_id { get; set; }
        [DisplayName("Book Status")]
        [Required(ErrorMessage = "Please Enter Book Status")]
        public string is_free { get; set; }
        [DisplayName("PDF")]
        public string upload_pdf { get; set; }
        public HttpPostedFileBase ImageUpload { get; set; }
        public HttpPostedFileBase PdfUpload { get; set; }



        public virtual tbl_author tbl_author { get; set; }

    public virtual tbl_language tbl_language { get; set; }

    public virtual tbl_publisher tbl_publisher { get; set; }

        

    }

}
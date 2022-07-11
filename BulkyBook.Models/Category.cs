using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [DisplayName("Display Order")]
    [Range(1, 100, ErrorMessage = "주문 번호는 1에서 100까지의 숫자 안에서 지정이 가능합니다.")] //범위 최소값과 최대값 지정 
    public int DisplayOrder { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

}

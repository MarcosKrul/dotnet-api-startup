using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Dtos.Comment
{
    public class DeleteCommentRequestDto : UserAuthenticatedInfos
    {
        public int Id { get; set; }
    }
}

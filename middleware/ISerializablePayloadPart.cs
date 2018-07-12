using System.IO;
using System.Threading.Tasks;

namespace Middleware
{
	public interface ISerializablePayloadPart
	{
		string Extension { get; }
		Task Serialize(Stream target);
	}
}
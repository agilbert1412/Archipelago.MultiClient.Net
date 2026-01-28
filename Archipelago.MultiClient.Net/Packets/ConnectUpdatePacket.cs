using Archipelago.MultiClient.Net.Converters;
using Archipelago.MultiClient.Net.Enums;
using Newtonsoft.Json;

namespace Archipelago.MultiClient.Net.Packets
{
    public class ConnectUpdatePacket : ArchipelagoPacketBase
    {
        public override ArchipelagoPacketType PacketType => ArchipelagoPacketType.ConnectUpdate;

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonConverter(typeof(IntEnumConverter))]
		[JsonProperty("items_handling")]
        public ItemsHandlingFlags? ItemsHandling { get; set; }
    }
}

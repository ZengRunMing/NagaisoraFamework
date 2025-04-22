using System.IO;
using System.Text;

namespace NagaisoraFamework.DataFileSystem
{
	public static class PlayerDataSystem
    {
        public static byte[] WritePlayerData(PlayerData[][] playerdatas)
        {
            MemoryStream memory = new();
            BinaryWriter binaryWriter = new(memory, Encoding.UTF8, true);

            binaryWriter.Write(playerdatas.Length);

			foreach (PlayerData[] datas in playerdatas)
            {
				binaryWriter.Write(datas.Length);

				foreach (PlayerData data in datas)
                {
                    byte[] buffer = data.ToBinary();

					binaryWriter.Write(buffer.Length);
                    binaryWriter.Write(buffer);
				}
            }

            return memory.ToArray();
        }

		public static PlayerData[][] ReadPlayerData(byte[] bytes)
		{
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            MemoryStream memory = new(bytes);
			BinaryReader binaryReader = new(memory, Encoding.UTF8, true); 

			PlayerData[][] datas = new PlayerData[binaryReader.ReadInt32()][];

            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = new PlayerData[binaryReader.ReadInt32()];
             
                for (int j = 0; j < datas[i].Length; j++)
                {
                    byte[] buffer = binaryReader.ReadBytes(binaryReader.ReadInt32());
                    datas[i][j] = PlayerData.FromBinary(buffer);
                }
            }

			return datas;
		}
	}
}
using System;
using System.IO;
using System.Security.Principal;

namespace AppHarbor.Web.Security
{
	public class AuthenticationTicket
	{
		private readonly int _version;
		private readonly Guid _id;
		private readonly bool _persistent;
		private DateTime _issueDate;
		private readonly string _name;
		private readonly byte[] _userData;

		private AuthenticationTicket(byte[] data)
		{
			using (var memoryStream = new MemoryStream(data))
			{
				using (var binaryReader = new BinaryReader(memoryStream))
				{
					_version = binaryReader.ReadInt32();
					_id = new Guid(binaryReader.ReadBytes(16));
					_persistent = binaryReader.ReadBoolean();
					_issueDate = DateTime.FromBinary(binaryReader.ReadInt64());
					_name = binaryReader.ReadString();
					
					var tagLength = binaryReader.ReadInt16();
					if (tagLength == 0)
					{
						_userData = null;
					}
					else
					{
						_userData = binaryReader.ReadBytes(tagLength);
					}
				}
			}
		}

		public AuthenticationTicket(int version, Guid id, bool persistent, string name, byte[] userData = null)
		{
			_version = version;
			_id = id;
			_persistent = persistent;
			_name = name;
			_userData = userData;
			_issueDate = DateTime.UtcNow;
		}

		public IPrincipal GetPrincipal()
		{
			var identity = new CookieIdentity(this);
			return new GenericPrincipal(identity, null);
		}

		public byte[] Serialize()
		{
			using (var memoryStream = new MemoryStream())
			{
				using (var binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(_version);
					binaryWriter.Write(_id.ToByteArray());
					binaryWriter.Write(_persistent);
					binaryWriter.Write(_issueDate.ToBinary());
					binaryWriter.Write(_name);
					if (_userData == null)
					{
						binaryWriter.Write((short)0);
					}
					else
					{
						binaryWriter.Write((short)_userData.Length);
						binaryWriter.Write(_userData);
					}
				}
				return memoryStream.ToArray();
			}
		}

		public static AuthenticationTicket Deserialize(byte[] data)
		{
			return new AuthenticationTicket(data);
		}

		public void Renew()
		{
			_issueDate = DateTime.UtcNow;
		}

		public bool IsExpired(TimeSpan validity)
		{
			return _issueDate.Add(validity) <= DateTime.UtcNow;
		}

		public DateTime IssueDate
		{
			get
			{
				return _issueDate;
			}
		}

		public bool Persistent
		{
			get
			{
				return _persistent;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public int Version
		{
			get
			{
				return _version;
			}
		}

		public Guid Id
		{
			get
			{
				return _id;
			}
		}

		public byte[] UserData
		{
			get
			{
				return _userData;
			}
		}
	}
}

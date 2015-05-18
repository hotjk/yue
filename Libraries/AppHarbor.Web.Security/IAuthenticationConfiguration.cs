﻿using System;
using System.Web.Security;

namespace AppHarbor.Web.Security
{
	public interface IAuthenticationConfiguration
	{
		string CookieName { get; }
		bool SlidingExpiration { get; }
		TimeSpan Timeout { get; }
		string LoginUrl { get; }
		string EncryptionAlgorithm { get; }
		byte[] EncryptionKey { get; }
        byte[] EncryptionIV { get; }
		string ValidationAlgorithm { get; }
		byte[] ValidationKey { get; }
		bool RequireSSL { get; }
        string CookiePath { get; }
	}
}

﻿using System.IO;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Tests.TestSupport;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;

namespace ICSharpCode.SharpZipLib.Tests.Zip
{
	[TestFixture]
	public class ZipStreamAsyncTests
	{
#if NETCOREAPP3_1_OR_GREATER
		[Test]
		[Category("Zip")]
		public async Task WriteZipStreamAsync ()
		{
			await using var ms = new MemoryStream();
			
			await using (var outStream = new ZipOutputStream(ms){IsStreamOwner = false})
			{
				await outStream.PutNextEntryAsync(new ZipEntry("FirstFile"));
				await Utils.WriteDummyDataAsync(outStream, 12);

				await outStream.PutNextEntryAsync(new ZipEntry("SecondFile"));
				await Utils.WriteDummyDataAsync(outStream, 12);
			}

			ZipTesting.AssertValidZip(ms);
		}
		
		[Test]
		[Category("Zip")]
		public async Task WriteZipStreamWithAesAsync()
		{
			await using var ms = new MemoryStream();
			var password = "f4ls3p0s1t1v3";
			
			await using (var outStream = new ZipOutputStream(ms){IsStreamOwner = false, Password = password})
			{
				await outStream.PutNextEntryAsync(new ZipEntry("FirstFile"){AESKeySize = 256});
				await Utils.WriteDummyDataAsync(outStream, 12);

				await outStream.PutNextEntryAsync(new ZipEntry("SecondFile"){AESKeySize = 256});
				await Utils.WriteDummyDataAsync(outStream, 12);
			}
			
			ZipTesting.AssertValidZip(ms, password);
		}
		
		[Test]
		[Category("Zip")]
		public async Task WriteZipStreamWithZipCryptoAsync()
		{
			await using var ms = new MemoryStream();
			var password = "f4ls3p0s1t1v3";
			
			await using (var outStream = new ZipOutputStream(ms){IsStreamOwner = false, Password = password})
			{
				await outStream.PutNextEntryAsync(new ZipEntry("FirstFile"){AESKeySize = 0});
				await Utils.WriteDummyDataAsync(outStream, 12);

				await outStream.PutNextEntryAsync(new ZipEntry("SecondFile"){AESKeySize = 0});
				await Utils.WriteDummyDataAsync(outStream, 12);
			}
			
			ZipTesting.AssertValidZip(ms, password, false);
		}
#endif
	}
}

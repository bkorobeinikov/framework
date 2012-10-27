namespace Bobasoft.Security
{
	public interface IEncryption<TOut, TIn1, TIn2>
	{
		//======================================================
		#region _Methods_

		TOut Encrypt(TIn1 param1, TIn2 param2);
		bool TryDecrypt(TOut input, out TIn1 param1, out TIn2 param2);

		#endregion
	}
}
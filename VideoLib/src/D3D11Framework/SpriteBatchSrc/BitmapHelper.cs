using System.Drawing;

namespace VideoLib.src.D3D11Framework
{
	public static class BitmapHelper
	{
		public static void CopyRegionIntoImage(Bitmap srcBitmap, System.Drawing.Rectangle srcRegion,
			ref Bitmap destBitmap, Rectangle destRegion)
		{
			using (System.Drawing.Graphics grD = System.Drawing.Graphics.FromImage(destBitmap))
			{
				grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
			}
		}
	}
}
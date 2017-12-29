using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public struct DevThreshold
	{
		public System.Collections.Generic.List<ThresholdFlg> ThresholdsFlg;
		public System.Collections.Generic.List<ThresholdFlg> UIEditFlg;
		public System.Collections.Generic.List<stru_CommRange> currentThresholds;
		public System.Collections.Generic.List<stru_CommRange> voltageThresholds;
		public System.Collections.Generic.List<stru_CommRange> powerThresholds;
		public System.Collections.Generic.List<stru_CommRange> powerDissThresholds;
		public System.Collections.Generic.List<stru_CommRange> tempThresholds;
		public System.Collections.Generic.List<stru_CommRange> HumiThresholds;
		public System.Collections.Generic.List<stru_CommRange> PressThresholds;
		public DevCommonThreshold commonThresholds;
		public void copy(DevThreshold src)
		{
			this.ThresholdsFlg = new System.Collections.Generic.List<ThresholdFlg>();
			for (int i = 0; i < src.ThresholdsFlg.Count; i++)
			{
				ThresholdFlg src2 = src.ThresholdsFlg[i];
				ThresholdFlg item = default(ThresholdFlg);
				item.copy(src2);
				this.ThresholdsFlg.Add(item);
			}
			this.UIEditFlg = new System.Collections.Generic.List<ThresholdFlg>();
			for (int j = 0; j < src.UIEditFlg.Count; j++)
			{
				ThresholdFlg src2 = src.UIEditFlg[j];
				ThresholdFlg item = default(ThresholdFlg);
				item.copy(src2);
				this.UIEditFlg.Add(item);
			}
			this.currentThresholds = new System.Collections.Generic.List<stru_CommRange>();
			for (int k = 0; k < src.currentThresholds.Count; k++)
			{
				stru_CommRange src3 = src.currentThresholds[k];
				stru_CommRange item2 = default(stru_CommRange);
				item2.copy(src3);
				this.currentThresholds.Add(item2);
			}
			this.voltageThresholds = new System.Collections.Generic.List<stru_CommRange>();
			for (int l = 0; l < src.voltageThresholds.Count; l++)
			{
				stru_CommRange src3 = src.voltageThresholds[l];
				stru_CommRange item2 = default(stru_CommRange);
				item2.copy(src3);
				this.voltageThresholds.Add(item2);
			}
			this.powerThresholds = new System.Collections.Generic.List<stru_CommRange>();
			for (int m = 0; m < src.powerThresholds.Count; m++)
			{
				stru_CommRange src3 = src.powerThresholds[m];
				stru_CommRange item2 = default(stru_CommRange);
				item2.copy(src3);
				this.powerThresholds.Add(item2);
			}
			this.powerDissThresholds = new System.Collections.Generic.List<stru_CommRange>();
			for (int n = 0; n < src.powerDissThresholds.Count; n++)
			{
				stru_CommRange src3 = src.powerDissThresholds[n];
				stru_CommRange item2 = default(stru_CommRange);
				item2.copy(src3);
				this.powerDissThresholds.Add(item2);
			}
			this.tempThresholds = new System.Collections.Generic.List<stru_CommRange>();
			for (int num = 0; num < src.tempThresholds.Count; num++)
			{
				stru_CommRange src3 = src.tempThresholds[num];
				stru_CommRange item2 = default(stru_CommRange);
				item2.copy(src3);
				this.tempThresholds.Add(item2);
			}
			this.HumiThresholds = new System.Collections.Generic.List<stru_CommRange>();
			for (int num2 = 0; num2 < src.HumiThresholds.Count; num2++)
			{
				stru_CommRange src3 = src.HumiThresholds[num2];
				stru_CommRange item2 = default(stru_CommRange);
				item2.copy(src3);
				this.HumiThresholds.Add(item2);
			}
			this.PressThresholds = new System.Collections.Generic.List<stru_CommRange>();
			for (int num3 = 0; num3 < src.PressThresholds.Count; num3++)
			{
				stru_CommRange src3 = src.PressThresholds[num3];
				stru_CommRange item2 = default(stru_CommRange);
				item2.copy(src3);
				this.PressThresholds.Add(item2);
			}
			this.commonThresholds = default(DevCommonThreshold);
			this.commonThresholds.copy(src.commonThresholds);
		}
	}
}

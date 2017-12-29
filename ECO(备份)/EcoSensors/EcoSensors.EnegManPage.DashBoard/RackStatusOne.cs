using System;
namespace EcoSensors.EnegManPage.DashBoard
{
	internal class RackStatusOne
	{
		private string m_racknm = "";
		private double m_power;
		private int m_intakeSSnum;
		private double m_intake_Tmin;
		private double m_intake_Tmax;
		private double m_intake_Tavg;
		private int m_exhaustSSnum;
		private double m_exhaust_Tmax;
		private double m_exhaust_Tavg;
		private int m_floorSSnum;
		private double m_floor_T;
		private double m_VEquipk;
		public string RackName
		{
			get
			{
				return this.m_racknm;
			}
		}
		public double Power
		{
			get
			{
				return this.m_power;
			}
			set
			{
				this.m_power = value;
			}
		}
		public int IntakeSSnum
		{
			get
			{
				return this.m_intakeSSnum;
			}
		}
		public int ExhaustSSnum
		{
			get
			{
				return this.m_exhaustSSnum;
			}
		}
		public int FloorSSnum
		{
			get
			{
				return this.m_floorSSnum;
			}
		}
		public double TIntake_min
		{
			get
			{
				return this.m_intake_Tmin;
			}
		}
		public double TIntake_diff
		{
			get
			{
				return this.m_intake_Tmax - this.m_intake_Tmin;
			}
		}
		public double TEquipk_avg
		{
			get
			{
				return this.m_exhaust_Tavg - this.m_intake_Tavg;
			}
		}
		public double TEquipk
		{
			get
			{
				return this.m_exhaust_Tmax - this.m_intake_Tmin;
			}
		}
		public double TFloor
		{
			get
			{
				return this.m_floor_T;
			}
		}
		public double TFloor_avg
		{
			get
			{
				return this.m_exhaust_Tavg - this.m_floor_T;
			}
		}
		public double VEquipk
		{
			get
			{
				return this.m_VEquipk;
			}
		}
		public RackStatusOne(string racknm, double power, int intakeSSnum, double intake_Tmin, double intake_Tmax, double intake_Tavg, int exhaustSSnum, double exhaust_Tmax, double exhaust_Tavg, int floorSSnum, double floor_Tavg, double VEquipk)
		{
			this.m_racknm = racknm;
			this.m_power = power;
			this.m_intakeSSnum = intakeSSnum;
			this.m_intake_Tmin = intake_Tmin;
			this.m_intake_Tmax = intake_Tmax;
			this.m_intake_Tavg = intake_Tavg;
			this.m_exhaustSSnum = exhaustSSnum;
			this.m_exhaust_Tmax = exhaust_Tmax;
			this.m_exhaust_Tavg = exhaust_Tavg;
			this.m_floorSSnum = floorSSnum;
			this.m_floor_T = floor_Tavg;
			this.m_VEquipk = VEquipk;
		}
	}
}

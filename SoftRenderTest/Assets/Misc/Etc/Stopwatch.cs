using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 区間の経過時間を計測、記録する.
class SectionTimes {

	public struct Section {

		public string			name;
		public Stopwatch.Time	time;

		public Section(string name, Stopwatch.Time time)
		{
			this.name = name;
			this.time = time;
		}
	}

	public List<Section>	sections = new List<Section>();

	public Stopwatch		stopwatch = new Stopwatch();

	// ================================================================ //

	public SectionTimes()
	{
		this.init();
	}
	public void		init()
	{
		this.stopwatch.init();
		this.sections.Clear();
	}
	public void		beginMeasure()
	{
		this.stopwatch.init();
		this.stopwatch.start();

		this.sections.Clear();

		this.mark("begin");
	}
	public void		endMeasure()
	{
		for(int i = 0;i < this.sections.Count - 1;i++) {

			Section		section = this.sections[i];

			section.time = this.sections[i].time - this.sections[i + 1].time;
		}

		if(this.sections.Count > 0) {

			Section		section = this.sections[this.sections.Count - 1];

			section.time = new Stopwatch.Time(0.0f);

			this.sections[this.sections.Count - 1] = section;
		}
	}
	public void		mark(string name)
	{
		this.sections.Add(new Section(name, this.stopwatch.getTime()));
	}
}

// ストップウォッチ.
public class Stopwatch {

	protected List<Status>				status = new List<Status>();


	public struct Time {

		public float	second;				// [second].

		public Time(float second)
		{
			this.second = second;
		}
		// 初期化.
		public void	init()
		{
			this.second = 0.0f;
		}

		public float	asPerFrame()
		{
			return(this.second*60.0f);
		}
		public float	asSecond()
		{
			return(this.second);
		}

		// たし算.
		public static Time add(Time x0, Time x1) {

			return(new Time(x0.second + x1.second));
		}
		// 引き算.
		public static Time subtract(Time x0, Time x1) {

			return(new Time(x0.second - x1.second));
		}
		public static Time operator+(Time x0, Time x1) 
		{
			return(Time.add(x0, x1));
		}
		public static Time operator-(Time x0, Time x1)
		{
			return(Time.subtract(x0, x1));
		}
	}

	public class Status {

		public bool		is_moving;
		public Time		start_time;			// [second].
		public Time		stop_time;			// [second].
	}

	// ================================================================ //

	public	Stopwatch()
	{
		this.init();
	}

	public void		init()
	{
		this.status.Clear();

		Status	status = new Status();

		status.is_moving = false;

		this.status.Add(status);
	}
	public void		reset()
	{
		this.get_current_status().start_time = Stopwatch.get_current_time();
	}

	public void		start()
	{
		this.get_current_status().is_moving = true;
		this.get_current_status().start_time = Stopwatch.get_current_time();
	}
	public void		stop()
	{
		this.get_current_status().is_moving = false;
		this.get_current_status().stop_time = Stopwatch.get_current_time();
	}

	// 現在の経過時間を取得する.
	public Stopwatch.Time		getTime()
	{
		Stopwatch.Time	passage_time;

		if(this.get_current_status().is_moving) {
			passage_time = Stopwatch.Time.subtract(Stopwatch.get_current_time(), this.get_current_status().start_time);
		} else {

			passage_time = Stopwatch.Time.subtract(this.get_current_status().stop_time, this.get_current_status().start_time);
		}

		return(passage_time);
	}

	public void		push()
	{
		this.status.Add(this.status[this.status.Count - 1]);
	}

	public Stopwatch.Time		pop()
	{
		Stopwatch.Time	time = new Time(0.0f);

		if(this.status.Count > 1) {

			time = this.getTime();

			this.status.RemoveAt(this.status.Count - 1);
		}

		return(time);
	}

	// push() してからの時間.
	public Stopwatch.Time	getInterval()
	{
		Stopwatch.Time	interval;

		interval = this.getTime();

		return(interval);

	}

	// ================================================================ //

	protected Status	get_current_status()
	{
		return(this.status[this.status.Count - 1]);
	}

	protected static Time	get_current_time()
	{
		return(new Time(UnityEngine.Time.realtimeSinceStartup));
	}

}


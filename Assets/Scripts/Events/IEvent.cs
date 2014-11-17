using UnityEngine;
using System.Collections;


public enum EventType{
	Bomb,
	Blimp,
	StraightRockShower,
	LittleRockShower,
	MapSpin,
	PointLights,
	EnemyEvent,
	HealthPowerUpEvent,
	Idle,
	NUM_TYPES
};


// this is an interface for the event.
public interface IEvent{
	
	void Begin();

	/// <summary>
	/// End is where you must do your End Logic for your event. Once this is done you MUST tell 
	/// the EventController what the NextState should be by doing calling EventController.EventEnd (EventType running, EventType nextState, float delay = 0);
	/// </summary>
	void End();

	/// <summary>
	/// Allows the Event to do OnDestroy housekeeping. Recommended that End() is called in here.
	/// </summary>
	void OnDestroy();
}

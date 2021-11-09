using UnityEngine;

// this abstract class inhirit from Monobehaviour
public abstract class ObserverAbstraction<T, W> : MonoBehaviour where T : ISubject<T, W>
{
	// every observer (UI...) need to subscribe the the Subject (HealSystem) the Subject is
	// protected because we want to use it only in the Observes Classes and no need to attach
	protected T SubjectRef { get; set; }

	// every subscriber need a ref to the Subject and a callBack function to execute since this
	// code is the same for every Observer we can use the Virtual keyWord to implement it
	public virtual void Subsribe(T Subject)
	{
		// save the HealthSystem instance into this Observer
		SubjectRef = Subject;
		// subscribe the Observer to the Subject ( this is execute first thing in the
		// PlayerStats )
		Subject.EventListner += OnSubjectEventChanges;
	}

	// since each Observer will have a different logic implementation we use the keyWord
	// abstract
	public abstract void OnSubjectEventChanges(W Subject);
}
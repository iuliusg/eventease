using System.ComponentModel.DataAnnotations;
namespace EventEase.Services;
public class EventInfo {
 public int Id {get;set;}
 [Required, StringLength(100, MinimumLength=2)] public string Name {get;set;} = "";
 [Range(typeof(DateTime), "2026-01-01", "2100-12-31")] public DateTime Date {get;set;}
 [Required, StringLength(120, MinimumLength=2)] public string Location {get;set;} = "";
}
public class RegistrationInput {
 [Required, StringLength(80, MinimumLength=2)] public string Name {get;set;} = "";
 [Required, EmailAddress, StringLength(160)] public string Email {get;set;} = "";
}
public record Attendee(int EventId, string Name, string Email) {public bool Attended {get;set;}}
public class EventStore {
 public List<EventInfo> Events {get;} = [
 new() {Id=1, Name="Creative Connections", Date=DateTime.Today.AddDays(30), Location="Riverside Hall"},
 new() {Id=2, Name="Technology Exchange", Date=DateTime.Today.AddDays(45), Location="Innovation Centre"},
 new() {Id=3, Name="Community Celebration", Date=DateTime.Today.AddDays(60), Location="City Gardens"}];
 public List<Attendee> Attendees {get;} = [];
 public RegistrationInput Session {get;private set;} = new();
 public EventInfo? Find(int id) => Events.Find(e=>e.Id==id);
 public bool Register(int id, RegistrationInput input) {
 var clean = new RegistrationInput {Name=input.Name.Trim(), Email=input.Email.Trim()};
 if(Find(id) is null || !Validator.TryValidateObject(clean,new(clean),null,true) || Attendees.Any(a=>a.EventId==id && a.Email.Equals(clean.Email,StringComparison.OrdinalIgnoreCase))) return false;
 Attendees.Add(new(id,clean.Name,clean.Email)); Session=clean; return true;
 }
 public void ClearSession() => Session=new();
}

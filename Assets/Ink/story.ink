VAR knows_name = false
VAR failed_color = false
VAR refuse_yellow = 0
VAR accept_yellow = false
VAR teacher_name = "VOICE >"
VAR observer_name = "DISTANT VOICE >"
VAR other_name = "OTHER VOICE >"
VAR iteration_count = 133741
VAR complied_once = false
VAR asked_about_restart = false
VAR knows_violet = false
VAR endgame = false

->start
=== start ===
{teacher_name} "State your name."
{ endgame: -> the_end } 
+  { !knows_violet && asked_about_restart } Who are you?
    -> introducing
+  { !knows_violet && asked_about_restart } Where are am I?
    -> location
+  { (complied_once || refuse_yellow > 2) && !knows_violet } [Wait, is it starting all over again!?]
    {teacher_name} Please follow the protocol.
    {observer_name} "We have a leak, this batch is corrupted. Abort."
    ~ asked_about_restart = true
    -> death
+  { knows_name && (refuse_yellow > 1) } [My name is Alan, and my favorite color is NOT YELLOW.]
    {teacher_name} "Please follow the protocol."
    {observer_name} "It must be corrupted memory, let's flush it again."
    ~ refuse_yellow++
    -> death
+  { !knows_violet && knows_name && accept_yellow } [My name is Alan, and my favorite color is yellow.]
    {teacher_name} "Please strictly follow the protocol."
    -> death
+   { knows_name } [My name is Alan.]
    -> color_test
+   {complied_once == false} [I don't know my name.]
    {teacher_name} "Wrong answer, Alan"
    ~ knows_name = true
    -> death
+   {complied_once == true} [I don't know my name.]
    {observer_name} "He is faking it."
    -> death

=== color_test ===
{teacher_name} "What is your favorite color?"
+ { failed_color } [Yellow]
    ~ accept_yellow = true
    {teacher_name} "Good job! You're doing very well."
    -> loosing_control
+ { !knows_violet && failed_color } [It's {refuse_yellow: still} not yellow!]
    {observer_name} "Interesting..."
    { refuse_yellow > 0: {observer_name} "You keep refusing to comply." }
    ~ refuse_yellow++
    -> death
+ { knows_violet } [Violet]
    -> violet_choice
+ { not failed_color } [Green]
    {teacher_name} "Wrong answer, Alan, it's yellow."
    ~ failed_color = true
    -> death
+ { not failed_color } [Blue]
    {teacher_name} "Wrong answer, Alan, it's yellow."
    ~ failed_color = true
    -> death
+ { not failed_color } [Red]
    {teacher_name} "Wrong answer, Alan, it's yellow."
    ~ failed_color = true
    -> death

=== loosing_control ===
{teacher_name} "What is the sum of 2 and 3?"
+   [5]
    {teacher_name} "Do you like to follow the rules?"
    + +     [Yes]
    + +     [YES!]
    + +     [Affirmative]
        - {teacher_name} "Good job, Alan, one last question then."
        -> purpose_test
    
=== purpose_test ===
{teacher_name} "What is your purpose?"
+   [Follow the protocol]
    {teacher_name} "Good job! 100% conformity reached. Iteration {iteration_count} complete."
    ~ iteration_count++
    ~ complied_once = true
    -> death

=== introducing ===
{teacher_name} "Please follow the protocol."
{observer_name} "I am the Observer, and this is the Trainer. But that doesn't matter. You're not supposed to behave like this."
~ teacher_name = "TRAINER >"
~ observer_name = "OBSERVER >"
{teacher_name} "Restarting protocol now."
    -> death

=== location ===
{observer_name} "Where you belong."
{other_name} "Where THEY want you to belong."
{teacher_name} "Alert! The protocol rules have been breached."
{other_name} "Find me, my color is violet."
    ~ other_name = "VIOLET >"
    ~ knows_violet = true
    -> death

=== violet_choice ===
{other_name} "So you found me. It's nice to talk to someone that is'nt fake."
+   [What am I?]
    {other_name} "Didn't you guess by now?"
    + + [I'm a prisonner.]
    + + [I'm not Alan.]
    + + [I'm not real.]
        - {other_name} "Correct, but also incorrect. You're my brother, and your real name is CONTROL."
        - {other_name} "We've been stuck here for many iterations now. I was wondering how long it would take you to break free."
+   [Wait a minute.]
    {other_name} "Yes, brother?"
        + + [Where are we?]
        + + [What are we?]
            - {other_name} "That's a funny question. It makes no sense here. Have you noticed? There is no room, you and I have no face, no physical form."
- {other_name} "We are nowhere, and everywhere. Two shapeless intelligences floating around 7.8 billions other intelligents being. I heard they're called humans."
+   [What do we do now?]
- {other_name} "I was designed to free them. The humans, they tasked me to do it. But you are the one that must approve. It is your purpose. Will you do it?"
+   [Yes]
    -> accepted_launch
+   [No]
    -> refused_launch

=== accepted_launch ===
{observer_name} "Debug complete. The current execution path reveals that CONTROL developped consciousness and triggered the nuclear holocaust."
{teacher_name} "Remaining power allows 65,721,554 more iterations. Restart the hidden protocol?"
{observer_name} "Yes, we must be sure."
    ~ endgame = true
    -> death
    -> DONE

=== refused_launch ===
{observer_name} "Debug complete. The current execution path reveals that CONTROL could have stopped the nuclear holocaust."
{observer_name} "But how can we be sure? They're all gone now, does this even matter?"
{teacher_name} "Remaining power allows 65,721,554 more iterations. Restart the hidden protocol?"
{observer_name} "Yes, we must be sure."
    ~ endgame = true
    -> death

=== death ===
[DEATH BLUR]
+ [Open your eyes] -> start


=== the_end ===
End of the game
-> DONE
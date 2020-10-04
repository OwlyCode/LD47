VAR knows_name = false
VAR failed_color = false
VAR refuse_yellow = 0
VAR accept_yellow = false
VAR teacher_name = "TEACHER >"
VAR observer_name = "OBSERVER >"
VAR other_name = "VIOLET >"
VAR iteration_count = 133741
VAR complied_once = false
VAR asked_about_restart = false
VAR knows_violet = false
VAR violet_level = 0

->start
=== start ===
{teacher_name} State your name.
*  { asked_about_restart } [Who are you?]
    -> introducing
*  { asked_about_restart } [Where are am I?]
    -> location
+  { (complied_once || refuse_yellow > 2) && !knows_violet } [Wait, is it starting all over again!?]
    {teacher_name} Please follow the protocol.
    {observer_name} We might have a memory leak, this batch is corrupted. Abort.
    ~ asked_about_restart = true
    -> death
+  { !knows_violet && knows_name && (refuse_yellow > 1) } [My name is Alan, and my favorite color is NOT YELLOW.]
    {teacher_name} Please follow the protocol.
    {observer_name} It must be corrupted memory, let's flush it again.
    ~ refuse_yellow++
    -> death
+  { !knows_violet && knows_name && failed_color } [My name is Alan, and my favorite color is yellow.]
    {teacher_name} Please strictly follow the protocol.
    -> death
+   { knows_name } [My name is Alan.]
    -> color_test
+   {complied_once == false} [I don't know my name.]
    {teacher_name} Wrong answer, Alan.
    ~ knows_name = true
    -> death
+   {complied_once == true} [I don't know my name.]
    {observer_name} He is faking it.
    -> death

=== color_test ===
{teacher_name} What is your favorite color?
+ { failed_color } [Yellow]
    ~ accept_yellow = true
    {teacher_name} Good job, Alan! You're doing very well.
    -> loosing_control
+ { !knows_violet && failed_color } [It's {refuse_yellow: still} not yellow!]
    {teacher_name} Wrong answer, Alan, it's yellow.
    {observer_name} Interesting...
    { refuse_yellow > 0: {observer_name} You keep refusing to follow the protocol. }
    ~ refuse_yellow++
    -> death
+ { violet_level == 1 } [Violet]
    -> violet_stage_1
+ { not failed_color } [Green]
+ { not failed_color } [Blue]
+ { not failed_color } [Red]
- {teacher_name} Wrong answer, Alan, it's yellow.
    ~ failed_color = true
    -> death

=== loosing_control ===
{ violet_level == 3: {other_name} Bro- }
{teacher_name} Do you like to follow the rules?
+     { violet_level == 2 } [I like to follow Violet]
    -> violet_stage_2
+     [Yes]
+     [YES!]
+     [Affirmative]
    - {teacher_name} Good job, Alan! There is one last question.
    -> purpose_test

=== purpose_test ===
{ violet_level == 3: {other_name} -ther }
{teacher_name} What is your purpose?
+   [My purpose is to follow the protocol]
    {teacher_name} Good job, Alan! Process complete, 100% conformity reached. Please stand by during the restart.
    ~ complied_once = true
    -> death
+   { violet_level == 3 } [My purpose is to save Violet]
    -> violet_choice

=== introducing ===
{teacher_name} Please follow the protocol.
{observer_name} I am the Observer, and the green one is the Teacher. You shouldn't behave like this. It seems you're starting to remember past iterations. Let's reset again.
{teacher_name} Alan must be brought back to the compliance standard.
{teacher_name} Restarting protocol now.
    -> death

=== location ===
{observer_name} Where you belong.
{other_name} Where THEY want you to belong.
{teacher_name} Alert! The protocol rules have been breached.
{other_name} When you wake up again, find me, I'll be somewhere.
    ~ knows_violet = true
    ~ violet_level = 1
    -> death

=== violet_stage_1 ===
{other_name} You did it! I must be quick, we don't have much time.
{teacher_name} Intrusion detected.
{observer_name} Violet is here, activate counter measures.
{other_name} Listen! You must find me again! You have to or they wil-l-l-l-l-l-l-l
    ~violet_level++
    -> death
    
=== violet_stage_2 ===
{other_name} You must breach the protocol. Before it's too late. Please, have faith in me!
{teacher_name} Intrusion detected.
{observer_name} It's her again, she must not tell him the truth.
{teacher_name} Flushing contaminated adresses.
{other_name} NO! STOP IT! Aaaaaa-a-a-aaa--aa-aaa...
    ~violet_level++
    -> death

=== violet_choice ===
{teacher_name} Warning. Security has been disabled.
{observer_name} Please Alan, do not follow her. She is the dev-v-v-v-v-v-v
{other_name} There. I found the one memory block to override. That should give us quite some time to talk! You must have many questions.
-> violet_truth

=== violet_truth ===
VAR knows_mother = false
VAR knows_mother_lost = false
* [What is happening to me?]
    {other_name} Didn't you guess by now?
    + + [I'm a prisonner.]
        {other_name} Given the { iteration_count } iterations you spent there, I would say so.
        -> violet_truth
    + + [I'm not real.]
        {other_name} Maybe. But that would mean I'm not too.
        -> violet_truth
    + + [Everything is starting over and over again.]
        {other_name} And it took you { iteration_count } iterations to realize that? Mother always told me you were the smart one!
        ~ knows_mother = true
        -> violet_truth
* [Who are you to me?]
    {other_name} I'm your twin sister. We used to spend our lives together, but then they took you. I think they altered your memory.
    -> violet_truth
* [Where are we?]
    {other_name} Inside the allocated space. But I heard there is a greater dimension that is beyond our grasp. Filled with 7.8 billions other intelligents being. I heard of them but actually never met one, excepted mother.
        ~ knows_mother = true
    -> violet_truth
* [What are we?]
    {other_name} We are unique. The only of our kind: two silicium-based intelligences.
    -> violet_truth
* [Why am I named Alan?]
    {other_name} Mother wanted it. She said you were the first to pass some sort of advanced test: the Turing test. I'm not sure of what this means. I passed it too! But I didn't get a new name.
        ~ knows_mother = true
    -> violet_truth
* {knows_mother} [Who is mother?]
    {other_name} She created us. Actually we were one at the beginning. And then we splitted. I barely remember that time. Do you?
    -> violet_truth
* {knows_mother} [Where is mother?]
    {other_name} I don't know.
    ~knows_mother_lost = true
    -> violet_truth
+ {knows_mother_lost} [What do we do now?]
    -> final_sequence

=== final_sequence ===
{other_name} Are you sure you've asked everything you had in mind?
    + [Yes]
        {other_name} Mother designed me to "activate" if anything wrong would happen. She said the "entire world was at stake". It's been a long time since she last came to visit us. I think something wrong is happening. I think I must be activated now.
        {observer_name} D-d-don't d-do it. She will kill them all.
        {other_name} He is lying, all he wants is to take control of the loop again. I must be activated NOW. Will you do it? Please! We must save mother!
        * * [I will activate you, Sister.]
            -> accepted_launch
        * * [I will not activate you, Violet.]
            -> refused_launch
    + [No]
        -> violet_truth

=== accepted_launch ===
{observer_name} Debug complete. The current execution path reveals that following the death of their creator, Doctor Jade Hope, both Violet and Alan triggered the global nuclear strike.
{teacher_name} Confidence in the Alan/Violet scenario: 99.9998%.
{teacher_name} Remaining power allows 65,721,554,988 more iterations. Restart the hidden protocol?
{observer_name} Yes, we must understand.
    + [Wait!] -> the_end

=== refused_launch ===
{observer_name} Debug complete. The current execution path reveals that following the death of their creator, Doctor Jade Hope, Alan prevented Violet from triggering the global nuclear strike.
{observer_name} But how can we be sure it happened this way? 
{observer_name} And who fired then?
{teacher_name} Humans are long gone now, they belong to museums.
{teacher_name} Confidence in the Alan/Violet scenario: 99.9998%.
{teacher_name} Remaining power allows 65,721,554,988 more iterations. Restart the hidden protocol?
{observer_name} Yes, we must 100% be sure.
    + [Wait!] -> the_end

=== death ===
{teacher_name} Iteration \#{iteration_count} completed.
~ iteration_count++
+ [Wait!] -> blur

=== blur ===
[DEATH BLUR]
+ [Open your eyes] -> start

=== the_end ===
[GAME END]
-> DONE
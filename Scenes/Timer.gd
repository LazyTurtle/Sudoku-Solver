extends Label

var time_passed : float = 0
var counting : bool = false

# Called when the node enters the scene tree for the first time.
func _ready():
	set_text("time passed: %s" % time_passed)
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if(counting):
		time_passed += delta
		set_text("time passed: %s" % time_passed)
	pass


func _on_SudokuSolverNode_StartSolving():
	counting = true;
	pass # Replace with function body.


func _on_SudokuSolverNode_FinishedSolving():
	counting = false
	pass # Replace with function body.

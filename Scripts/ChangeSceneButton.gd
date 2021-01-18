extends Button
export(String, FILE) var destination_scene

func _ready():
	pass # Replace with function body.

func _pressed():
	SceneLoader.goto_scene(destination_scene)

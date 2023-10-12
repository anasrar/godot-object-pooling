class_name GDPlayer
extends CharacterBody2D

var _can_fire_bullet: bool = true
var _timer_cooldown_fire_bullet: Timer = Timer.new()

@export_range(0.01, 999, 0.01, "or_greater") var cooldown_fire_bullet = 0.2
@export var path_pool_bullets: NodePath

func _ready():
    _timer_cooldown_fire_bullet.wait_time = cooldown_fire_bullet
    _timer_cooldown_fire_bullet.timeout.connect(func(): _can_fire_bullet = true)
    add_child(_timer_cooldown_fire_bullet)

func _physics_process(_delta):
    rotation = (get_viewport().get_mouse_position() - global_position).angle()
    if(Input.is_action_pressed("fire_bullet") && _can_fire_bullet):
        var pool_bullets = get_node(path_pool_bullets) as GDPool

        var bullet = pool_bullets.get_item() as GDBullet
        bullet.spawn(rotation)
        _can_fire_bullet = false;
        _timer_cooldown_fire_bullet.start()

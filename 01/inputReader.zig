const std = @import("std");
const Allocator = std.mem.Allocator;
const ArrayList = std.ArrayList;

pub fn parseList(alloc: Allocator, input: std.io.AnyReader) ![2]ArrayList(i32) {
    var first = ArrayList(i32).init(alloc);
    var second = ArrayList(i32).init(alloc);

    var line_buffer: [16:0]u8 = undefined;
    while (input.readUntilDelimiter(&line_buffer, '\n')) |line| {
        var strs = std.mem.splitSequence(u8, line, "   ");
        const match: i32 = std.fmt.parseInt(i32, strs.next().?, 10) catch return .{ first, second };
        first.append(match) catch unreachable;
        second.append(std.fmt.parseInt(i32, strs.next().?, 10) catch unreachable) catch unreachable;
    } else |_| {
        return .{ first, second };
    }
    unreachable;
}

const std = @import("std");
const Allocator = std.mem.Allocator;
const page_allocator = std.heap.page_allocator;
const ArrayList = std.ArrayList;

fn parseList(alloc: Allocator, input: []const u8) [2]ArrayList(i32) {
    var lines = std.mem.split(u8, input, "\n");
    var first = ArrayList(i32).init(alloc);
    defer first.deinit();
    var second = ArrayList(i32).init(alloc);
    defer second.deinit();

    while (lines.next()) |line| {
        var strs = std.mem.split(u8, line, " ");
        const match: i32 = std.fmt.parseInt(i32, strs.next().?, 10) catch |err| blk: {
            std.debug.print("how do you actually handle an error in this language?\n{any}\n", .{err});
            break :blk 47;
        };
        std.debug.print("look: {any}\n", .{match});
        first.append(match) catch unreachable;
        second.append(std.fmt.parseInt(i32, strs.next().?, 10) catch unreachable) catch unreachable;
    }

    return .{ first, second };
}

pub fn main() void {
    const alloc = std.heap.page_allocator;

    const input: []const u8 = "3   4\n4   3\n2   5\n1   3\n3   9\n3   3";
    const parsed = parseList(alloc, input);
    std.debug.print("{}\n", .{parsed[0].items[0]});
}

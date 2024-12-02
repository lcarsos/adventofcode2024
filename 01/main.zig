const std = @import("std");
const Allocator = std.mem.Allocator;
const page_allocator = std.heap.page_allocator;
const ArrayList = std.ArrayList;

fn parseList(alloc: Allocator, input: []const u8) ![2]ArrayList(i32) {
    var lines = std.mem.split(u8, input, "\n");
    const first = ArrayList(i32).init(alloc);
    defer first.deinit();
    const second = ArrayList(i32).init(alloc);
    defer second.deinit();

    while (lines.next()) |line| {
        var strs = std.mem.split(u8, line, " ");
        const match: i32 = try std.fmt.parseInt(i32, strs.next().?, 10);
        std.debug.print("look: {any}\n", .{match});
        //first.append(try std.fmt.parseInt(i32, match, 10));
        //second.append(std.fmt.parseInt(i32, strs.next().?, 10));
        break;
    }

    return .{first, second};
}

pub fn main() void {
    const alloc = std.heap.page_allocator;

    const input: []const u8 = "3   4\n4   3\n2   5\n1   3\n3   9\n3   3";
    const parsed = try parseList(alloc, input) catch {
        std.debug.print("how do you actually handle an error in this language?\n", .{});
        .{0};
    };
    std.debug.print("{}\n", .{parsed[0].items[0]});
}
"use client"

import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle
} from "@/components/ui/alert-dialog"
import { api } from "@/lib/api"
import { useState } from "react"

export function ApproveRejectDialog({
  open,
  onClose,
  type,
  name,
  userId
}: {
  open: boolean
  type: "approve" | "reject"
  name: string
  userId: number
  onClose: (success: boolean) => void
}) {
  const [loading, setLoading] = useState(false)
  const handleConfirm = async () => {
    setLoading(true)
    try {
      await api.approveUser(userId)
      onClose(true)
    } catch (err) {
      console.error(err)
      onClose(false) 
    } finally {
      setLoading(false)
    }
  }
  return (
    <AlertDialog open={open}>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>
            {type === "approve" ? "Approve User" : "Reject User"}
          </AlertDialogTitle>

          <AlertDialogDescription>
            Are you sure you want to {type} <b>{name}</b>?
          </AlertDialogDescription>
        </AlertDialogHeader>

        <AlertDialogFooter>
          <AlertDialogCancel onClick={() => onClose(false)}>
            Cancel
          </AlertDialogCancel>

          <AlertDialogAction
            onClick={handleConfirm}
            disabled={loading}
            className={type === "approve" ? "bg-green-600" : "bg-red-600"}
          >
            {loading ? "Saving..." : type === "approve" ? "Approve" : "Reject"}
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  )
}

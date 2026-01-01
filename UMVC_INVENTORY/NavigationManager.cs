using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UMVC_INVENTORY
{
    /// <summary>
    /// Centralized navigation manager that maintains a stack-based navigation history
    /// Provides browser-like back button functionality for form navigation
    /// </summary>
    public static class NavigationManager
    {
        // Stack to maintain navigation history (LIFO - Last In First Out)
        private static Stack<Form> navigationStack = new Stack<Form>();
        
        // Flag to track if we're currently navigating (to prevent Login form from showing)
        private static bool isNavigating = false;
        
        // Flag to track if we're logging out (to ensure Login form shows)
        private static bool isLoggingOut = false;

        /// <summary>
        /// Navigates to a new form by hiding the current form and showing the target form
        /// </summary>
        /// <param name="currentForm">The currently active form to be hidden</param>
        /// <param name="targetForm">The target form to navigate to</param>
        public static void NavigateTo(Form currentForm, Form targetForm)
        {
            if (currentForm == null || targetForm == null)
            {
                throw new ArgumentNullException("Forms cannot be null");
            }

            // Prevent navigating to the same form instance
            if (currentForm == targetForm)
            {
                return;
            }

            // Remove current form from navigation stack if it exists (to avoid duplicates)
            RemoveFromHistory(currentForm);

            // Push current form onto navigation stack BEFORE hiding it
            // This allows us to navigate back to it later
            navigationStack.Push(currentForm);

            // Set navigation flag to prevent Login form from showing
            isNavigating = true;
            
            // Mark the form as being closed for navigation (using Tag property)
            currentForm.Tag = "NAVIGATING";

            // Hide the current form (instead of closing, so it can be restored later)
            currentForm.Hide();

            // Show the target form
            targetForm.Show();
            targetForm.Activate();

            // Reset navigation flag after a short delay
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 50; // 50ms delay - enough for events to process
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                timer.Dispose();
                isNavigating = false;
            };
            timer.Start();
        }
        
        /// <summary>
        /// Checks if we're currently navigating between forms
        /// This prevents Login form from showing when forms are closed during navigation
        /// </summary>
        /// <returns>True if currently navigating, False otherwise</returns>
        public static bool IsNavigating()
        {
            return isNavigating;
        }
        
        /// <summary>
        /// Checks if a form is being closed for navigation purposes
        /// </summary>
        /// <param name="form">The form to check</param>
        /// <returns>True if the form is being closed for navigation, False otherwise</returns>
        public static bool IsFormNavigating(Form form)
        {
            return form != null && form.Tag != null && form.Tag.ToString() == "NAVIGATING";
        }

        /// <summary>
        /// Navigates back to the previous form in the navigation history
        /// </summary>
        /// <param name="currentForm">The currently active form to be hidden or closed</param>
        /// <returns>True if navigation back was successful, False if there's no previous form</returns>
        public static bool NavigateBack(Form currentForm)
        {
            if (navigationStack.Count == 0)
            {
                // No previous form in history - cannot navigate back
                return false;
            }

            // Hide or close the current form
            if (currentForm != null)
            {
                currentForm.Hide();
            }

            // Pop the previous form from the stack
            Form previousForm = navigationStack.Pop();

            // Show and activate the previous form
            previousForm.Show();
            previousForm.Activate();

            return true;
        }

        /// <summary>
        /// Clears the entire navigation history stack
        /// Useful when logging out or resetting the navigation state
        /// </summary>
        public static void ClearHistory()
        {
            // Clear all forms from the stack
            while (navigationStack.Count > 0)
            {
                Form form = navigationStack.Pop();
                // Dispose forms that are no longer needed
                if (form != null && !form.IsDisposed)
                {
                    form.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Marks that a logout is in progress
        /// This ensures the Login form will be shown when the dashboard closes
        /// </summary>
        public static void SetLoggingOut(Form form)
        {
            isLoggingOut = true;
            isNavigating = false; // Ensure navigation flag is off
            if (form != null)
            {
                form.Tag = "LOGOUT"; // Mark form as being closed for logout
                
                // Wire up FormClosed event to show Login form when this form closes
                form.FormClosed += (s, args) =>
                {
                    if (IsLoggingOut() || IsFormLoggingOut(form))
                    {
                        // Show Login form when logout is confirmed
                        Login.ShowLoginForm();
                        ResetLogoutFlag();
                    }
                };
            }
        }
        
        /// <summary>
        /// Checks if we're currently logging out
        /// </summary>
        /// <returns>True if logging out, False otherwise</returns>
        public static bool IsLoggingOut()
        {
            return isLoggingOut;
        }
        
        /// <summary>
        /// Checks if a form is being closed for logout
        /// </summary>
        /// <param name="form">The form to check</param>
        /// <returns>True if the form is being closed for logout, False otherwise</returns>
        public static bool IsFormLoggingOut(Form form)
        {
            return form != null && form.Tag != null && form.Tag.ToString() == "LOGOUT";
        }
        
        /// <summary>
        /// Resets the logout flag (called after Login form is shown)
        /// </summary>
        public static void ResetLogoutFlag()
        {
            isLoggingOut = false;
        }

        /// <summary>
        /// Checks if there's a previous form in the navigation history
        /// </summary>
        /// <returns>True if navigation back is possible, False otherwise</returns>
        public static bool CanNavigateBack()
        {
            return navigationStack.Count > 0;
        }

        /// <summary>
        /// Gets the count of forms in the navigation history
        /// </summary>
        /// <returns>The number of forms in the navigation stack</returns>
        public static int GetHistoryCount()
        {
            return navigationStack.Count;
        }

        /// <summary>
        /// Removes a specific form from the navigation stack without navigating to it
        /// Useful when a form is closed externally and should be removed from history
        /// </summary>
        /// <param name="formToRemove">The form to remove from the navigation stack</param>
        public static void RemoveFromHistory(Form formToRemove)
        {
            if (formToRemove == null)
            {
                return;
            }

            // Create a temporary stack to rebuild without the removed form
            Stack<Form> tempStack = new Stack<Form>();

            // Pop all forms and rebuild stack without the target form
            while (navigationStack.Count > 0)
            {
                Form form = navigationStack.Pop();
                if (form != formToRemove)
                {
                    tempStack.Push(form);
                }
            }

            // Restore the stack
            while (tempStack.Count > 0)
            {
                navigationStack.Push(tempStack.Pop());
            }
        }
    }
}
